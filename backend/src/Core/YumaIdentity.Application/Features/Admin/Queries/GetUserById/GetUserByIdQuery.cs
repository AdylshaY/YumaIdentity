namespace YumaIdentity.Application.Features.Admin.Queries.GetUserById
{
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using System;

    public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
