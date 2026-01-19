using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.API.Data;      
using MyWebApp.Shared.Models; 

namespace sellphone.Controllers
{
    [Route("api/[controller]")] // Đường dẫn sẽ là: api/ProductFS
    [ApiController]
    public class ProductFSController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public ProductFSController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: Lấy danh sách sản phẩm (api/ProductFS)
        [HttpGet]
        public async Task<ActionResult<List<ProductFS>>> GetAll()
        {
            return await _context.ProductFS.ToListAsync();
        }

        // 2. POST: Thêm sản phẩm mới (api/ProductFS)
        [HttpPost]
        public async Task<ActionResult<ProductFS>> Create([FromBody] ProductFS product)
        {
            _context.ProductFS.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
        // 3. GET: Lấy chi tiết 1 sản phẩm theo ID (Để điền vào form sửa)
        // URL: api/ProductFS/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductFS>> GetById(int id)
        {
            var product = await _context.ProductFS.FindAsync(id);
            if (product == null) return NotFound("Không tìm thấy sản phẩm");
            return product;
        }

        // 4. PUT: Cập nhật sản phẩm
        // URL: api/ProductFS/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductFS product)
        {
            // Kiểm tra an toàn: ID trên URL phải khớp với ID trong gói tin
            if (id != product.Id) return BadRequest("ID không khớp!");

            // Đánh dấu trạng thái là "Đã bị sửa đổi"
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra kỹ nếu sản phẩm bị xóa mất trong lúc đang sửa
                if (!_context.ProductFS.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent(); // Trả về 204 (Thành công nhưng không có nội dung trả về)
        }
    }
}