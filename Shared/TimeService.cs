using System;
using System.Collections.Generic;
using System.ServiceModel;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace GrpcSample.Proto
{
    [ServiceContract]
    public interface ITimeService
    {
        [OperationContract]
        IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default);
    }

    [ProtoContract]
    public class TimeResult
    {
        [ProtoMember(1, DataFormat = DataFormat.WellKnown)]
        public DateTime Time { get; set; }
    }
}