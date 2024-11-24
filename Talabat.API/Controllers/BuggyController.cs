using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabt.Repository.Data;

namespace Talabat.API.Controllers
{
    public class BuggyController : APIBaseController
    {
        private readonly MyDbContext _dbContext;

        public BuggyController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbContext.Products.Find(100);
            if(Product is null) return NotFound(new ApiResponse(404));
            return Ok(Product);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturn = Product.ToString();
            return Ok(ProductToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetbadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
