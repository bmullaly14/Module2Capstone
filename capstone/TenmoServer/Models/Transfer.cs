

using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; } 

        public int TransferTypeId { get; set; }

        public int TransferStatusId { get; set; }

        [Required(ErrorMessage = "The 'Account From' field must not be blank.")]
        public int AccountFrom { get; set;}

        [Required(ErrorMessage = "The 'Account To' field must not be blank.")]
        public int AccountTo { get; set;}

        [Range(0.01, double.PositiveInfinity, ErrorMessage = "The field `Amount` must be greater than 0.")]
        public decimal Amount { get; set;}


        
    }
}
