namespace Bookshop.Application.Features.Response.Contracts
{
    public interface IResponseFactory<T> where T : BaseResponse, new()
    {
        T CreateResponse();
    }
}
