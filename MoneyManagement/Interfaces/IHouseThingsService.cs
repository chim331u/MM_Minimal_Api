using MoneyManagement.Models.HouseThings;

namespace MoneyManagement.Interfaces
{
    public interface IHouseThingsService
    {
        Task<ICollection<HouseThings>> GetActiveHouseThingsList();
        Task<ICollection<HouseThings>> GetActiveHouseThingsListByRoom(int id);
        Task<ICollection<HouseThings>> GetHistoryHouseThingsList(int historyId);
        Task<HouseThings> GetHouseThings(int houseThingsId);
        Task<HouseThings> AddHouseThings(HouseThings houseThings);
        Task<HouseThings> RenewHouseThings(HouseThings houseThings);
        Task<HouseThings> UpdateHouseThings(HouseThings houseThings);
        Task<HouseThings> DeleteHouseThings(HouseThings houseThings);

        Task<ICollection<HouseThingsRooms>> GetActiveHouseThingsRoomsList();
        Task<HouseThingsRooms> GetHouseThingsRooms(int houseThingsRoomsId);
        Task<HouseThingsRooms> AddHouseThingsRooms(HouseThingsRooms houseThingsRooms);
        Task<HouseThingsRooms> UpdateHouseThingsRooms(HouseThingsRooms houseThingsRooms);
        Task<HouseThingsRooms> DeleteHouseThingsRooms(HouseThingsRooms houseThingsRooms);
    }
}
