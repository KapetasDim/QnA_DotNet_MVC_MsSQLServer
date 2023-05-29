using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QnA_DotNet_MVC_MsSQLServer.Models
{
    [Table("BlockTable")]
    public partial class BlockTable
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("block_title")]
        [StringLength(512)]
        public string BlockTitle { get; set; } = null!;
        [Column("text", TypeName = "text")]
        public string Text { get; set; } = null!;
        [Column("date_posted", TypeName = "datetime")]
        public DateTime? DatePosted { get; set; }
        [Column("isAnswer")]
        public bool IsAnswer { get; set; }
        [Column("is_answer_to_block_id")]
        public int IsAnswerToBlockId { get; set; }
    }
}
