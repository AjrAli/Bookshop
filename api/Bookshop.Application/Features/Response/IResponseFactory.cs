namespace Bookshop.Application.Features.Response
{
    public interface IResponseFactory<T> where T : BaseResponse, new()
    {
        T CreateResponse();
    }
}
