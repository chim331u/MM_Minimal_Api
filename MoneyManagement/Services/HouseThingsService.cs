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
                var result = await _context.HouseThings.Include(i => i.Room).Where(x => x.IsActive).OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ICollection<HouseThings>> GetActiveHouseThingsListByRoom(int id)
        {
            try
            {
                var result = await _context.HouseThings.Include(x => x.Room).Where(x => x.IsActive && x.Room.Id == id).OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<ICollection<HouseThings>> GetHistoryHouseThingsList(int historyId)
        {
            try
            {
                var result = await _context.HouseThings.Include(x => x.Room).Where(x => x.IsActive == false && x.HistoryId == historyId)
                    .OrderByDescending(x => x.PurchaseDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<HouseThings> UpdateHouseThings(HouseThings item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.HouseThings.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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

                var result = await _context.HouseThings.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<HouseThings> RenewHouseThings(HouseThings item)
        {
            //item = new HouseThings;


            try
            {
                var oldItem = await _context.HouseThings.FindAsync(item.Id);
                var room = await _context.houseThingsRooms.FindAsync(item.Room.Id);
               
                await DeleteHouseThings(oldItem);
                item.Id = 0;
                await AddHouseThings(item);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<HouseThings> DeleteHouseThings(HouseThings item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.HouseThings.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }


        #endregion

        #region House Things Rooms

        public async Task<ICollection<HouseThingsRooms>> GetActiveHouseThingsRoomsList()
        {
            try
            {
                var result = await _context.houseThingsRooms.Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<HouseThingsRooms> UpdateHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;

                var result = _context.houseThingsRooms.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<HouseThingsRooms> AddHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                var result = await _context.houseThingsRooms.AddAsync(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }

        public async Task<HouseThingsRooms> DeleteHouseThingsRooms(HouseThingsRooms item)
        {
            try
            {
                item.LastUpdatedDate = DateTime.Now;
                item.IsActive = false;

                var result = _context.houseThingsRooms.Update(item);
                await _context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;

            }

        }
        #endregion
    }
}
