using Microsoft.EntityFrameworkCore;
using MoneyManagement.AppContext;
using MoneyManagement.Interfaces;
using MoneyManagement.Models.HouseThings;

namespace MoneyManagement.Services
{
    public class HouseThingsService : IHouseThingsService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<HouseThingsService> _logger;

        public HouseThingsService(ApplicationContext context, ILogger<HouseThingsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region House Things 

        public async Task<ICollection<HouseThings>> GetActiveHouseThingsList()
        {
            try
            {
                var result = await _context.HouseThings
                    .Include(i => i.Room).Where(x => x.IsActive).OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving active HouseThings list: {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<HouseThings>> GetActiveHouseThingsListByRoom(int id)
        {
            try
            {
                var result = await _context.HouseThings
                    .Include(x => x.Room)
                    .Where(x => x.IsActive && x.Room.Id == id).OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving active HouseThings list by room: {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<HouseThings>> GetHistoryHouseThingsList(int historyId)
        {
            try
            {
                var result = await _context.HouseThings.Include(x => x.Room)
                    .Where(x => x.IsActive == false && x.HistoryId == historyId)
                    .OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving history HouseThings list: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThings> GetHouseThings(int houseThingsId)
        {
            try
            {
                var result = await _context.HouseThings.FindAsync(houseThingsId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving HouseThings with ID {houseThingsId}: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThings> UpdateHouseThings(HouseThings item)
        {
            try
            {
                var existingItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                {
                    _logger.LogWarning($"Unable to find HouseThings with ID {item.Id} for update.");
                    return null;
                }

                existingItem.Cost = item.Cost;
                existingItem.Description = item.Description;
                existingItem.IsActive = item.IsActive;
                existingItem.LastUpdatedDate = DateTime.Now;
                existingItem.Name = item.Name;
                existingItem.PurchaseDate = item.PurchaseDate;
                existingItem.HistoryId = item.HistoryId;
                existingItem.ItemType = item.ItemType;
                existingItem.Model = item.Model;
                existingItem.Note = item.Note;

                _context.HouseThings.Update(existingItem);

                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating HouseThings with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThings> AddHouseThings(HouseThings item)
        {
            try
            {
                var room = await _context.houseThingsRooms.FindAsync(item.Room.Id);

                item.CreatedDate = DateTime.Now;
                item.IsActive = true;
                item.Room = room;

                await _context.HouseThings.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding HouseThings: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThings> RenewHouseThings(HouseThings item)
        {
            //item = new HouseThings;

            try
            {
                var oldItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();

                if (oldItem == null)
                {
                    _logger.LogWarning($"Unable to find HouseThings with ID {item.Id} to renew.");
                    return null;
                }

                await DeleteHouseThings(oldItem);

                item.Id = 0;
                await AddHouseThings(item);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error renewing HouseThings with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThings> DeleteHouseThings(HouseThings item)
        {
            try
            {
                var existingItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == item.Id)
                    .FirstOrDefaultAsync();
                if (existingItem == null)
                {
                    _logger.LogWarning($"Unable to find HouseThings with ID {item.Id} for deletion.");
                    return null;
                }

                existingItem.LastUpdatedDate = DateTime.Now;
                existingItem.IsActive = false;

                _context.HouseThings.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting HouseThings with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region House Things Rooms

        public async Task<ICollection<HouseThingsRooms>> GetActiveHouseThingsRoomsList()
        {
            try
            {
                var result = await _context.houseThingsRooms.Where(x => x.IsActive)
                    .OrderByDescending(x => x.CreatedDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving active HouseThingsRooms list: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThingsRooms> GetHouseThingsRooms(int houseThingsRoomId)
        {
            try
            {
                var result = await _context.houseThingsRooms.FindAsync(houseThingsRoomId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving HouseThingsRooms with ID {houseThingsRoomId}: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThingsRooms> UpdateHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                var existingItem = await _context.houseThingsRooms.FindAsync(item.Id);

                if (existingItem == null)
                {
                    _logger.LogWarning($"Unable to find HouseThingsRooms with ID {item.Id} for update.");
                    return null;
                }

                existingItem.Color = item.Color;
                existingItem.Description = item.Description;
                existingItem.IsActive = item.IsActive;
                existingItem.Name = item.Name;
                existingItem.Icon = item.Icon;
                existingItem.Note = item.Note;
                existingItem.LastUpdatedDate = DateTime.Now;

                _context.houseThingsRooms.Update(existingItem);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating HouseThingsRooms with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThingsRooms> AddHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                await _context.houseThingsRooms.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding HouseThingsRooms: {ex.Message}");
                return null;
            }
        }

        public async Task<HouseThingsRooms> DeleteHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                var existingItem = await _context.houseThingsRooms.FindAsync(item.Id);
                if (existingItem == null)
                {
                    _logger.LogWarning($"Unable to find HouseThingsRooms with ID {item.Id} for deletion.");
                    return null;
                }
                
                
                
                existingItem.LastUpdatedDate = DateTime.Now;
                existingItem.IsActive = false;

                _context.houseThingsRooms.Update(existingItem);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting HouseThingsRooms with ID {item.Id}: {ex.Message}");
                return null;
            }
        }

        #endregion
    }
}