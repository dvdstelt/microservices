using System;
using MediatR;

namespace Shared.Notifications;

public class MyNotification : INotification
{
    public Guid MovieId { get; set; }
    public Guid TheaterId { get; set; }
    //
    public string MovieTitle { get; set; }
    public string TheaterName { get; set; }
}