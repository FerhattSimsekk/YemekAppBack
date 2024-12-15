﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;


namespace Application.Dtos.Restorans.Request
{
	public class RestoranUpdateDto
	{
		public string ad { get; set; } = null!;
		public string telefon { get; set; } = null!;
		public string adres { get; set; } = null!;
		public string hakkinda { get; set; } = null!;
		public IFormFile? resimUrl { get; set; }
		public bool acikMi { get; set; }
		public string? calismaSaatleri { get; set; }
		public string? tahminiTeslimatSüresi { get; set; }
		public Status status { get; set; }
		public int KategoriRestoranId { get; set; }

	}
}