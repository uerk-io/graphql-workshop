using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQL.Attendees
{
    [ExtendObjectType("Subscription")]
    public class AttendeeSubscriptions
    {
        [Subscribe(With = nameof(SubscribeToOnAttendeeCheckedInAsync))]
        public SessionAttendeeCheckIn OnAttendeeCheckedIn(
            [param: ID] int sessionId,
            [EventMessage] int attendeeId) =>
            new SessionAttendeeCheckIn(attendeeId, sessionId);

        public async ValueTask<ISourceStream<int>> SubscribeToOnAttendeeCheckedInAsync(
            int sessionId,
            [Service] ITopicEventReceiver eventReceiver,
            CancellationToken cancellationToken) =>
            await eventReceiver.SubscribeAsync<string, int>(
                "OnAttendeeCheckedIn_" + sessionId, cancellationToken);
    }
}