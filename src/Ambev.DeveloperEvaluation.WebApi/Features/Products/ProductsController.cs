//using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
//using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
//using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
//using Ambev.DeveloperEvaluation.WebApi.Common;
//using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
//using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
//using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
//using AutoMapper;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
//{
//    /// <summary>
//    /// Controller for managing user operations
//    /// </summary>
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductsController : BaseController
//    {
//        private readonly IMediator _mediator;
//        private readonly IMapper _mapper;

//        /// <summary>
//        /// Initializes a new instance of ProductsController
//        /// </summary>
//        /// <param name="mediator">The mediator instance</param>
//        /// <param name="mapper">The AutoMapper instance</param>
//        public ProductsController(IMediator mediator, IMapper mapper)
//        {
//            _mediator = mediator;
//            _mapper = mapper;
//        }

//        /// <summary>
//        /// Creates a new product
//        /// </summary>
//        /// <param name="request">The user creation request</param>
//        /// <param name="cancellationToken">Cancellation token</param>
//        /// <returns>The created user details</returns>
//        [HttpPost]
//        [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status201Created)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> CreateProduct([FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
//        {
//            var validator = new CreateProductRequestValidator();
//            var validationResult = await validator.ValidateAsync(request, cancellationToken);

//            if (!validationResult.IsValid)
//                return BadRequest(validationResult.Errors);

//            var command = _mapper.Map<CreateProductCommand>(request);
//            var response = await _mediator.Send(command, cancellationToken);

//            return Created(string.Empty, new ApiResponseWithData<UpdateProductResponse>
//            {
//                Success = true,
//                Message = "Product created successfully",
//                Data = _mapper.Map<UpdateProductResponse>(response)
//            });
//        }

//        /// <summary>
//        /// Retrieves a user by their ID
//        /// </summary>
//        /// <param name="id">The unique identifier of the user</param>
//        /// <param name="cancellationToken">Cancellation token</param>
//        /// <returns>The user details if found</returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
//        {
//            var request = new GetCartRequest { Id = id };
//            var validator = new GetProductRequestValidator();
//            var validationResult = await validator.ValidateAsync(request, cancellationToken);

//            if (!validationResult.IsValid)
//                return BadRequest(validationResult.Errors);

//            var command = _mapper.Map<GetProductCommand>(request.Id);
//            var response = await _mediator.Send(command, cancellationToken);

//            return Ok(new ApiResponseWithData<GetCartResponse>
//            {
//                Success = true,
//                Message = "Product retrieved successfully",
//                Data = _mapper.Map<GetCartResponse>(response)
//            });
//        }

//        /// <summary>
//        /// Retrieves a user by their ID
//        /// </summary>
//        /// <param name="id">The unique identifier of the user</param>
//        /// <param name="cancellationToken">Cancellation token</param>
//        /// <returns>The user details if found</returns>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetListProduct([FromRoute] Guid id, CancellationToken cancellationToken)
//        {
//            var request = new GetCartRequest { Id = id };
//            var validator = new GetProductRequestValidator();
//            var validationResult = await validator.ValidateAsync(request, cancellationToken);

//            if (!validationResult.IsValid)
//                return BadRequest(validationResult.Errors);

//            var command = _mapper.Map<GetProductCommand>(request.Id);
//            var response = await _mediator.Send(command, cancellationToken);

//            return Ok(new ApiResponseWithData<GetCartResponse>
//            {
//                Success = true,
//                Message = "Product retrieved successfully",
//                Data = _mapper.Map<GetCartResponse>(response)
//            });
//        }

//        /// <summary>
//        /// Deletes a user by their ID
//        /// </summary>
//        /// <param name="id">The unique identifier of the user to delete</param>
//        /// <param name="cancellationToken">Cancellation token</param>
//        /// <returns>Success response if the user was deleted</returns>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
//        {
//            var request = new DeleteCartRequest { Id = id };
//            var validator = new DeleteProductRequestValidator();
//            var validationResult = await validator.ValidateAsync(request, cancellationToken);

//            if (!validationResult.IsValid)
//                return BadRequest(validationResult.Errors);

//            var command = _mapper.Map<DeleteProductCommand>(request.Id);
//            await _mediator.Send(command, cancellationToken);

//            return Ok(new ApiResponse
//            {
//                Success = true,
//                Message = "Product deleted successfully"
//            });
//        }
//    }
//}
