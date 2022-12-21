using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Catalog.Data;
using Catalog.DTOs;
using Catalog.Domain;

namespace Catalog.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly ItemsRepositoryContract itemsRepository;

    public ItemsController(ItemsRepositoryContract itemsRepository)
    {
        this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public IEnumerable<ItemDTO> GetAllItems() => this.itemsRepository
        .GetAllItems()
        .Select(ItemDTO.fromEntity);

    [HttpGet("{id}")]
    public ActionResult<ItemDTO> GetItemById(Guid id)
    {
        var result = this.itemsRepository.GetItemById(id);
        return result is not null
            ? ItemDTO.fromEntity(result)
            : NotFound();
    }

    [HttpPost("create")]
    public ActionResult<ItemDTO> SaveItem(CreateItemDTO createItemDTO)
    {
        Item mappedItem = new()
        {
            Name = createItemDTO.Name,
            Price = createItemDTO.Price,
            CreatedAt = DateTimeOffset.Now,
        };

        var result = this.itemsRepository.SaveItem(mappedItem);
        return CreatedAtAction(nameof(GetItemById), new { id = result.Id }, ItemDTO.fromEntity(result));
    }

    [HttpPut("update")]
    public ActionResult<ItemDTO> UpdateItem(UpdateItemDTO updateItemDTO)
    {
        Item currentForm = itemsRepository.GetItemById(updateItemDTO.Id);

        if (currentForm is null) return NotFound();

        this.itemsRepository.UpdateItem(new()
        {
            Id = currentForm.Id,
            Name = updateItemDTO.Name,
            Price = updateItemDTO.Price,
            CreatedAt = currentForm.CreatedAt
        });

        return NoContent();
    }

    [HttpDelete("delete/{itemId}")]
    public ActionResult<ItemDTO> DeleteItem(Guid itemId)
    {
        Item currentForm = itemsRepository.GetItemById(itemId);

        if (currentForm is null) return NotFound();

        this.itemsRepository.DeleteItem(currentForm.Id);

        return NoContent();
    }
}