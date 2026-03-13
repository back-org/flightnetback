using System.Collections.Generic;
using System.Linq;
using Flunt.Notifications;

namespace Flight.Domain.Results;

/// <summary>
///     The result.
/// </summary>
public class Result : Notifiable<Notification>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> class.
    /// </summary>
    protected Result()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> class.
    /// </summary>
    /// <param name="notifications">The notifications.</param>
    protected Result(IReadOnlyCollection<Notification> notifications)
    {
        AddNotifications(notifications);
    }

    /// <summary>
    ///     Gets a value indicating whether success.
    /// </summary>
    public bool Success => !Notifications.Any();

    /// <summary>
    ///     Oks the.
    /// </summary>
    /// <returns>A Result.</returns>
    public static Result Ok()
    {
        return new Result();
    }

    /// <summary>
    ///     Errors the.
    /// </summary>
    /// <param name="notifications">The notifications.</param>
    /// <returns>A Result.</returns>
    public static Result Error(IReadOnlyCollection<Notification> notifications)
    {
        return new Result(notifications);
    }
}

/// <summary>
///     The result.
/// </summary>
public class Result<T> : Notifiable<Notification> where T : class
{
    /// <summary>
    ///     Prevents a default instance of the <see cref="Result" /> class from being created.
    /// </summary>
    /// <param name="obj">The obj.</param>
    private Result(T obj)
    {
        Object = obj;
    }

    /// <summary>
    ///     Prevents a default instance of the <see cref="Result" /> class from being created.
    /// </summary>
    /// <param name="notifications">The notifications.</param>
    private Result(IReadOnlyCollection<Notification> notifications)
    {
        Object = null;
        AddNotifications(notifications);
    }

    /// <summary>
    ///     Gets a value indicating whether success.
    /// </summary>
    public bool Success => !Notifications.Any();

    /// <summary>
    ///     Gets the object.
    /// </summary>
    public T Object { get; }

    /// <summary>
    ///     Oks the.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns>A Result.</returns>
    public static Result<T> Ok(T obj)
    {
        return new Result<T>(obj);
    }

    /// <summary>
    ///     Errors the.
    /// </summary>
    /// <param name="notifications">The notifications.</param>
    /// <returns>A Result.</returns>
    public static Result<T> Error(IReadOnlyCollection<Notification> notifications)
    {
        return new Result<T>(notifications);
    }
}