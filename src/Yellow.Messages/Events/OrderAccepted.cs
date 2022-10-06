﻿namespace Yellow.Messages.Events;

public class OrderAccepted
{
    public Guid OrderId { get; set; }
    public Guid MovieId { get; set; }
    public Guid TheaterId { get; set; }
    public string Time { get; set; }
    public int NumberOfTickets { get; set; }
}