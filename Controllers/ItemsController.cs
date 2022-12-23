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
    public async Task<IEnumerable<ItemDTO>> GetAllItems()
    {
        var items = await this.itemsRepository
            .GetAllItemsAsync();

        return items.Select(ItemDTO.FromEntity);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDTO>> GetItemById(Guid id)
    {
        var result = await this.itemsRepository.GetItemByIdAsync(id);

        return result is not null
            ? ItemDTO.FromEntity(result)
            : NotFound();
    }

    [HttpPost("create")]
    public async Task<ActionResult<ItemDTO>> SaveItem(CreateItemDTO createItemDTO)
    {
        Item mappedItem = new()
        {
            Id = Guid.NewGuid(),
            Name = createItemDTO.Name,
            Price = createItemDTO.Price,
            CreatedAt = DateTimeOffset.Now,
        };
        await itemsRepository.SaveItemAsync(mappedItem);

        return CreatedAtAction(nameof(GetItemById), new { id = mappedItem.Id }, ItemDTO.FromEntity(mappedItem));
    }

    [HttpPut("update")]
    public async Task<ActionResult<ItemDTO>> UpdateItem(UpdateItemDTO updateItemDTO)
    {
        Item currentForm = await itemsRepository.GetItemByIdAsync(updateItemDTO.Id);
        if (currentForm is null) return NotFound();
        await this.itemsRepository.UpdateItemAsync(new()
        {
            Id = currentForm.Id,
            Name = updateItemDTO.Name,
            Price = updateItemDTO.Price,
            CreatedAt = currentForm.CreatedAt
        });

        return NoContent();
    }

    [HttpDelete("delete/{itemId}")]
    public async Task<ActionResult<ItemDTO>> DeleteItem(Guid itemId)
    {
        Item currentForm = await itemsRepository.GetItemByIdAsync(itemId);
        if (currentForm is null) return NotFound();
        await itemsRepository.DeleteItemAsync(currentForm.Id);

        return NoContent();
    }
}