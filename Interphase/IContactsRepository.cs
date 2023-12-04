public interface IContactsRepository
{
    Task<IEnumerable<Contact>> GetContacts(string? search);
    void AddContact(Contact contact);
    Task<Contact?> GetContactByIdAsync(int id);
}