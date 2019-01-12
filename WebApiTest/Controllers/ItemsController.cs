using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiTest.DTO;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    public class ItemsController : ApiController
    {
        private AmazonContext db = new AmazonContext();

        // GET: api/Items
        public async Task<IHttpActionResult> GetItems()
        {
            var items = await db.Items.ToListAsync();

            List<ItemDto> itemsDto = new List<ItemDto>();
            foreach(var i in items)
            {
                ItemDto dto = new ItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    CreationDate = i.CreationDate
                };
                itemsDto.Add(dto);
            }
            return Ok(itemsDto);
        }

        // GET: api/Items/5
        [ResponseType(typeof(ItemDto))]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ItemDto dto = new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreationDate = item.CreationDate
            };

            return Ok(dto);
        }

        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutItem(int id, ItemDto item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            var element = db.Items.SingleOrDefault(dbItem => dbItem.Id == id);
            element.Name = item.Name;
            element.Description = item.Description;
            element.Price = item.Price;
            
            db.Entry(element).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Items
        [ResponseType(typeof(ItemDto))]
        public async Task<IHttpActionResult> PostItem(ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item = new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Description = itemDto.Description,
                CreationDate = itemDto.CreationDate,
                Price = itemDto.Price
            };

            db.Items.Add(item);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = itemDto.Id }, itemDto);
        }

        // DELETE: api/Items/5
        [ResponseType(typeof(ItemDto))]
        public async Task<IHttpActionResult> DeleteItem(int id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}