using ConferencePlanner.GraphQL.Common;
using ConferencePlanner.GraphQL.Data;
using GraphQL.Common;

namespace GraphQL.Speakers
{
    public class SpeakerPayloadBase : Payload
    {
        protected SpeakerPayloadBase(Speaker speaker)
        {
            Speaker = speaker;
        }

        protected SpeakerPayloadBase(IReadOnlyList<UserError> errors) : base(errors)
        {
        }

        public Speaker? Speaker { get; }
    }
}
