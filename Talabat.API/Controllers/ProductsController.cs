using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;
using Talabt.Core.Specifications;

namespace Talabat.API.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        #region Get All Products
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            /* var Products = await _productRepo.GetAllAsync();
             //OkObjectResult result = new OkObjectResult(Products);
             return Ok(Products);*/
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
           var MappedProducts = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);
            return Ok(MappedProducts);
        }
        #endregion
        #region Get Product By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromQuery] ProductSpecParams Params)
        {
            /*var Product = await _productRepo.GetByIdAsync(id);
            return Ok(Product);*/
            var Spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product,ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }
        #endregion
        #region Get All Types
        [HttpGet("Types")]
        public async Task <ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }
        #endregion
        #region Get All Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
        #endregion
    }
}
