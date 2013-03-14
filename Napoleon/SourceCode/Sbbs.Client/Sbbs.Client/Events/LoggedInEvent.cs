using System;
using Microsoft.Practices.Prism.Events;

namespace Sbbs.Client
{
    /// <summary>
    /// The event is raised when user is logged in.
    /// Thus his/her favorite boards should be load if any.
    /// </summary>
    public class LoggedInEvent: CompositePresentationEvent<EventArgs> { }
}
