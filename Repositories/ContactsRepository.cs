using Microsoft.EntityFrameworkCore;
public class ContactRepository : IContactsRepository
{
    private readonly ApplicationDbContext _context;

    public ContactRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetContacts(string? search = null)
    {
        IQueryable<Contact> query = _context.Contacts;
        if(!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Name.Contains(search));
        return await query.ToListAsync();
    }

    public void AddContact(Contact contact)
    {
        _context.Contacts.Add(contact);
        _context.SaveChanges();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }
}
