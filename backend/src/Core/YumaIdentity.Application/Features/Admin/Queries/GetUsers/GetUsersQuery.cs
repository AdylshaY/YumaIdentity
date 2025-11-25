namespace YumaIdentity.Application.Features.Admin.Queries.GetUsers
{
    using MediatR;
    using System.Collections.Generic;

    public class GetUsersQuery : IRequest<List<GetUsersResponse>>
    {
    }
}
