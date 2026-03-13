using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Notifications;

namespace Flight.Domain.Core.Abstracts;

[NotMapped]
public class BaseNotification : Notification
{
    public int Id { get; set; }
}