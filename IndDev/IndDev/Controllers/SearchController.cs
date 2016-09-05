using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using IndDev.Domain.Abstract;
using IndDev.Domain.ViewModels;

namespace IndDev.Controllers
{
    public class SearchController : ApiController
    {
        private readonly ISearchRepository _repository;

        public SearchController(ISearchRepository repository)
        {
            _repository = repository;
        }

        public IHttpActionResult GetProducts()
        {
            var result = _repository.GetProducts;
            return Ok(result.Take(1));
        }
        [HttpPost]
        public async Task<IHttpActionResult> PostSearch(SearchModel resultModel)
        {
            var result = await _repository.SearchResult(resultModel.SearchRequest);
            return result != null ? Ok(result) : (IHttpActionResult) BadRequest("По вашему запросу нет результата");
        }

        [HttpGet]
        public IHttpActionResult GetValue(int id)
        {
            return Ok(id+20);
        }
    }
}
