using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContactApp.Models;
using System.Security.Claims;

namespace ContactApp.Controllers;

public class HomeController : Controller
{
    // Add the Interface 
    private readonly IContactsRepository _contactRepository;

    public HomeController(IContactsRepository contactsRepository)
    {
        _contactRepository = contactsRepository;
    }

    public async Task<IActionResult> Index(string? search = null)
    {
        // Call for interface 
        var contacts = await _contactRepository.GetContacts(search);
        // User for form 
        // Retrieve user ID from claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;

        var viewModel = new ContactIndexViewModel
        {
            Contacts = contacts,
            UserId = userId
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(Contact contact)
    {

        // Fix Phone and Fax Vals
        if (!string.IsNullOrEmpty(contact.Phone))
        {
            contact.Phone = new String(contact.Phone.Where(Char.IsDigit).ToArray());
            ModelState.Remove("Phone");
        }

        if (!string.IsNullOrEmpty(contact.Fax))
        {
            contact.Fax = new String(contact.Fax.Where(Char.IsDigit).ToArray());
            ModelState.Remove("Fax");
        }

        if (ModelState.IsValid)
        {
            // To add todays date 
            contact.LastUpdatedDate = DateTime.Now;
            // Use your repository to add the new contact
            _contactRepository.AddContact(contact);
            return RedirectToAction("Index");
        }

        // Log or inspect the ModelState errors
        foreach (var error in ModelState)
        {
            // For validation purposes only
            Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
        }

        // If validation fails, return to the current view
        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Detail(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        if (contact == null)
            return NotFound();

        return View(contact);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
