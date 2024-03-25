using ApiApplication.Database.Entities;

namespace Domain.Entities
{
    public class AuditoriumModel
    {
        public int Id { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }
    }
}
