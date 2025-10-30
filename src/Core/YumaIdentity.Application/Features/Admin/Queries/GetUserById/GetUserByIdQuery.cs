namespace YumaIdentity.Application.Features.Admin.Queries.GetUserById
{
    using MediatR;
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
