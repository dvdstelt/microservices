// Select `demo` from 'run configuration' in Rider
// Make sure SMTP tool is running.
//   https://github.com/rnwood/smtp4dev/releases
//
// Break a leg ;-)

// ### TicketHub.cs
// Snippet#1 - Add to ctor
IMessageSession messageSession

// Snippet#2
// Create the message object
var order = new SubmitOrder
{
    OrderIdentifier = Guid.NewGuid(),
    TheaterIdentifier = Guid.Parse(ticket.TheaterId),
    MovieIdentifier = Guid.Parse(ticket.MovieId),
    Time = ticket.Time,
    NumberOfTickets = ticket.NumberOfTickets,
    UserId = Guid.Parse("218d92c4-9c42-4e61-80fa-198b22461f61"), // For now, no other users allowed ;-)
};

// Snippet#3
// Add connection identifier to message header
var sendOptions = new SendOptions();
sendOptions.SetHeader("SignalRConnectionId", userConnectionId);

// Snippet#4
// Have NServiceBus serialize it and send it using queues
await messageSession.Send(order, sendOptions);

// ### SubmitOrderHandler.cs in Yellow.Ticketing
// Snippet#5
public class SubmitOrderHandler : IHandleMessages<SubmitOrder> {}

// Snippet#6
public SubmitOrderHandler(YellowLiteDatabase db) => this.db = db;

// Snippet#7
var order = new Order
{
    Identifier = message.OrderIdentifier,
    MovieIdentifier = message.MovieIdentifier,
    TheaterIdentifier = message.TheaterIdentifier,
    UserIdentifier = message.UserId,
    NumberOfTickets = message.NumberOfTickets,
    MovieTime = message.Time
};
db.Insert(order);
return Task.CompletedTask;

// ### OrderPolicy.cs in Yellow.Ticketing
// Snippet#8
public class OrderPolicy : Saga<OrderSagaData>,
    IAmStartedByMessages<SubmitOrder>,
{    
}

// Snippet#9
public class OrderPolicy : Saga<OrderSagaData>,
    IAmStartedByMessages<SubmitOrder>,

// Snippet#10 at the bottom of the OrderPolicy
public class OrderSagaData : ContainSagaData
{
    public Guid OrderId { get; set; }
    public bool SeatsApproved { get; set; }
    public bool PaymentApproved { get; set; }
    public bool OrderPaid { get; set; }
    public Guid MovieId { get; set; }
    public int NumberOfTickets { get; set; }
    public Guid TheaterId { get; set; }
    public string Time { get; set; }
}

// Snippet#11 in ConfigureHowToFindSaga.
mapper.MapSaga(s => s.OrderId)
    .ToMessage<SubmitOrder>(m => m.OrderIdentifier)
    .ToMessage<SeatsDenied>(m => m.OrderId)
    .ToMessage<SeatsApproved>(m => m.OrderId)
    .ToMessage<OrderPaid>(m => m.OrderId);

// Snippet#12 in Handle(SubmitOrder ...)
Data.MovieId = message.MovieIdentifier;
Data.TheaterId = message.TheaterIdentifier;
Data.Time = message.Time;
Data.NumberOfTickets = message.NumberOfTickets;

await context.Publish(new OrderSubmitted()
{
    OrderId = message.OrderIdentifier,
    MovieId = message.MovieIdentifier,
    TheaterId = message.TheaterIdentifier,
    Time = message.Time,
    NumberOfTickets = message.NumberOfTickets
});

// Snippet#13
await RequestTimeout<OrderTimedOut>(context, TimeSpan.FromMinutes(2));

// Snippet#14 - Add to OrderPolicy
IHandleTimeouts<OrderTimedOut>,

// Snippet#15
public async Task Timeout(OrderTimedOut state, IMessageHandlerContext context)
{
    await context.Publish(new OrderDenied(Data.OrderId, "Ordering took too long."));
    MarkAsComplete();
}

// ### OrderSubmittedHandler.cs in Yellow.Ticketing
// Snippet#16
public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        // arrange payment
        await context.Publish(new OrderPaid() { OrderId = message.OrderId });
    }
}

// ### OrderPolicy.cs
// Snippet#17
IHandleMessages<OrderPaid>,

// Snippet#18
public async Task Handle(OrderPaid message, IMessageHandlerContext context)
{
    Data.OrderPaid = true;
    await VerifyProgress(context);
}

// Snippet#19
async Task VerifyProgress(IMessageHandlerContext context)
{
    if (!Data.OrderPaid || !Data.SeatsApproved)
        return;
}

// Snippet#20 in VerifyProgress()
var message = new OrderApproved()
{
    OrderId = Data.OrderId,
    MovieId = Data.MovieId,
    NumberOfTickets = Data.NumberOfTickets,
    TheaterId = Data.TheaterId,
    Time = Data.Time
};

await context.Publish(message);
MarkAsComplete();

// ### OrderSubmittedHandler.cs in Blue.Ticketing
// Snippet#21
public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    readonly BlueLiteDatabase db;
    
    public OrderSubmittedHandler(BlueLiteDatabase db) => this.db = db;
    
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {        
    }
}

// Snippet#22
var showing = db.Query<Showing>()
    .Where(v => v.MovieId == message.MovieId && v.TheaterId == message.TheaterId && v.Time == message.Time)
    .SingleOrDefault();

if (showing == null)
{
    await context.Publish(new SeatsApproved { OrderId = message.OrderId });
    return;
}

// Snippet#23
var allMatchingOrders = db.Query<Order>().Where(o =>
    o.MovieId == message.MovieId && o.TheaterId == message.TheaterId && o.Time == message.Time);
var totalTicketsOrdered = allMatchingOrders.Select(s => s.NumberOfTickets).ToList().Sum();
var totalTicketsLeft = showing.SeatsAvailable - totalTicketsOrdered;

// Snippet#24
if (totalTicketsLeft < message.NumberOfTickets)
{
    await context.Publish(new SeatsDenied() { OrderId = message.OrderId });
    return;
}

// Snippet#25
// All is good
await context.Publish(new SeatsApproved { OrderId = message.OrderId });

// Store this order
var orderCollection = db.Database.GetCollection<Order>();
orderCollection.Insert(new Order() { Identifier = message.OrderId ,MovieId = message.MovieId, TheaterId = message.TheaterId, Time = message.Time, NumberOfTickets = message.NumberOfTickets});

// ### OrderPolicy.cs
// Snippet#26
IHandleMessages<SeatsDenied>,
IHandleMessages<SeatsApproved>,

// Snippet#27
public async Task Handle(SeatsDenied message, IMessageHandlerContext context)
{
    // Oh oh!
    await context.Publish(new OrderDenied(Data.OrderId, "No seats left"));
    MarkAsComplete();
}

public async Task Handle(SeatsApproved message, IMessageHandlerContext context)
{
    // Now we wait for the payment approval.
    Data.SeatsApproved = true;
    await VerifyProgress(context);
}

// ### OrderApprovedHandler.cs in website
// Snippet#28
public class OrderApprovedHandler : IHandleMessages<OrderApproved>
{
    readonly IHubContext<TicketHub> ticketHubContext;
    readonly ILogger<OrderApprovedHandler> logger;
    readonly RedLiteDatabase db;

    public OrderApprovedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderApprovedHandler> logger, RedLiteDatabase db)
    {
        this.ticketHubContext = ticketHubContext;
        this.logger = logger;
        this.db = db;
    }
}

// Snippet#29
if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
{
    logger.LogError("Could not find SignalR ConnectionId from message headers");
    return;
}

// Snippet#30
//
// *** TODO: This needs to be fixed! This violates the service boundary!
//
var movie = db.Query<Red.Data.Entities.Movie>().Where(s => s.Identifier == message.MovieId).Single();
var theater = TheatersContext.GetTheaters().Single(s => s.Id == message.TheaterId);

var ticket = new
{
    success = true,
    message.OrderId,
    TheaterId = theater.Id.ToString(),
    Theater = theater.Name,
    MovieId = movie.Identifier.ToString(),
    MovieTitle = movie.Title,
    Time = message.Time,
    NumberOfTickets = message.NumberOfTickets
};

await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);

// ### OrderDeniedHandler
// Snippet#31
public class OrderDeniedHandler : IHandleMessages<OrderDenied>
{
    readonly IHubContext<TicketHub> ticketHubContext;
    readonly ILogger<OrderApprovedHandler> logger;

    public OrderDeniedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderApprovedHandler> logger)
    {
        this.ticketHubContext = ticketHubContext;
        this.logger = logger;
    }
    
    public async Task Handle(OrderDenied message, IMessageHandlerContext context)
    {
        logger.LogDebug("Oops, order denied.");
        
        if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
        {
            logger.LogError("Could not find SignalR ConnectionId from message headers");
            return;
        }

        var ticket = new
        {
            success = false,
            message.OrderId,
        };
        
        await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);
    }
}

//
// *** ViewModel Composition
//

// Start in /Views/Home/Index.cshtml

// ### FrontPageRequestsHandler.cs in Red.Data
// Snippet#32
public class FrontPageRequestsHandler : ICompositionRequestsHandler
{
}

// Snippet#33
readonly LiteRepository db;

public FrontPageRequestsHandler(RedLiteDatabase db) => this.db = db;

// Snippet#34
[HttpGet("/")]
public async Task Handle(HttpRequest request)
{
    var vm = request.GetComposedResponseModel();
}

// Snippet#35
var movies = db.Query<Movie>().ToList();
var availableMovies = MapToDictionary(movies);

vm.Movies = availableMovies.Values.ToList();

await vm.RaiseEvent(new AllMoviesLoaded() { AvailableMovies = availableMovies });

// Snippet#36
IDictionary<Guid, dynamic> MapToDictionary(IEnumerable<Movie> movies)
{
    var allMovies = new Dictionary<Guid, dynamic>();

    foreach (var movie in movies)
    {
        dynamic vm = new ExpandoObject();
        vm.Identifier = movie.Identifier;
        vm.UrlTitle = movie.UrlTitle;
        vm.Title = movie.Title;
        vm.Description = movie.Description;
        vm.Image = movie.Image;
        vm.Rating = movie.Rating;
        vm.Icons = movie.Icons;
        vm.MovieDetails = movie.MovieDetails;
        vm.ReleaseDate = movie.ReleaseDate;
        vm.PricePerTicket = movie.PricePerTicket;
        vm.TicketType = movie.TicketType;

        allMovies[movie.Identifier] = vm;
    }
    
    return allMovies;
}

// ### MovieDetailsSubscriber.cs in Blue.Data
// Snippet#37
public class MovieDetailsSubscriber : ICompositionEventsSubscriber
{
    readonly BlueLiteDatabase db;

    public MovieDetailsSubscriber(BlueLiteDatabase db) => this.db = db;

    [HttpGet("/")]
    public void Subscribe(ICompositionEventsPublisher publisher)
    {
        publisher.Subscribe<AllMoviesLoaded>((@event, request) =>
        {
            var moviesPopularity = db.Query<Movie>().ToList();

            foreach (var availableMovie in @event.AvailableMovies)
            {
                var movie = moviesPopularity.Single(s => s.Identifier == availableMovie.Key);
                availableMovie.Value.PopularityScore = movie.PopularityScore;
            }

            return Task.CompletedTask;
        });
    }
}

// ### MovieReviewsRequestsHandler.cs in Blue.Data
public class MovieReviewsRequestsHandler : ICompositionRequestsHandler
{
    readonly BlueLiteDatabase db;

    public MovieReviewsRequestsHandler(BlueLiteDatabase db) => this.db = db;
    
    [HttpGet("/reviews/{movieurl}")]
    public Task Handle(HttpRequest request)
    {
        var movieUrl = (string)request.HttpContext.GetRouteData().Values["movieurl"]!;
        var vm = request.GetComposedResponseModel();
        
        var movie = db.Query<Movie>().Where(s => s.UrlTitle == movieUrl).Single();
        var reviews = db.Query<Review>().Where(s => s.MovieIdentifier == movie.Identifier).ToList();

        vm.Reviews = reviews;
        
        return Task.CompletedTask;
    }
}