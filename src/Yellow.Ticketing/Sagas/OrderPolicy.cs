using Blue.Messages.Events;
using NServiceBus;
using Yellow.Messages.Commands;
using Yellow.Messages.Events;

namespace Yellow.Ticketing.Sagas;

public class OrderPolicy : Saga<OrderSagaData>,
    IAmStartedByMessages<SubmitOrder>,
    IHandleMessages<SeatsDenied>,
    IHandleMessages<SeatsApproved>,
    IHandleMessages<OrderPaid>,

IHandleTimeouts<OrderTimedOut>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(s => s.OrderId)
            .ToMessage<SubmitOrder>(m => m.OrderIdentifier)
            .ToMessage<SeatsDenied>(m => m.OrderId)
            .ToMessage<SeatsApproved>(m => m.OrderId)
            .ToMessage<OrderPaid>(m => m.OrderId);
    }

    public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
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

        await RequestTimeout<OrderTimedOut>(context, TimeSpan.FromSeconds(120));
    }

    public async Task Timeout(OrderTimedOut state, IMessageHandlerContext context)
    {
        if (!Data.OrderPaid || !Data.SeatsApproved)
            await context.Publish(new OrderDenied(Data.OrderId, "Ordering took too long."));
        MarkAsComplete();
    }

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

    public async Task Handle(OrderPaid message, IMessageHandlerContext context)
    {
        Data.OrderPaid = true;
        await VerifyProgress(context);
    }

    async Task VerifyProgress(IMessageHandlerContext context)
    {
        if (!Data.OrderPaid || !Data.SeatsApproved)
            return;

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
    }
}

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

public class OrderTimedOut
{
}