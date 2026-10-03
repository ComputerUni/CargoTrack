using CargoTrack.DTO.DTOs.CargosDtos;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using CargoTrack.Business.Extensions;

namespace CargoTrack.WebUI.Helpers
{
    public static class CargoPdfGenerator
    {
        public static byte[] Generate(ResultCargoDto cargo)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseSystemFonts = true;
            QuestPDF.Settings.ThrowOnMissingFontFamilies = false;

            var primaryColor = "#0f172a";
            var secondaryColor = "#475569";
            var lightBg = "#f8fafc";
            var borderColor = "#e2e8f0";
            var accentGreen = "#059669";

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9.5f).FontFamily("Arial").FontColor("#1e293b"));

                    page.Header().Column(headerCol =>
                    {
                        headerCol.Item().Row(row =>
                        {
                            row.RelativeItem(3).Column(col =>
                            {
                                col.Item().Row(r =>
                                {
                                    r.AutoItem().Text("CARGO").FontSize(22).Black().FontColor(primaryColor);
                                    r.AutoItem().Text("TRACK").FontSize(22).Light().FontColor(accentGreen);
                                });
                                col.Item().Text("Hızlı, Güvenli ve Akıllı Taşımacılık").FontSize(8.5f).FontColor(secondaryColor);
                            });

                            row.RelativeItem(2).AlignRight().Column(col =>
                            {
                                col.Item().Text("RESMİ TESLİMAT FİŞİ").FontSize(11).Bold().FontColor(secondaryColor);
                                col.Item().Text(cargo.TrackCode).FontSize(14).Black().FontColor(primaryColor);

                                col.Item().PaddingTop(3).Container()
                                    .Background(lightBg)
                                    .Border(1)
                                    .BorderColor(borderColor)
                                    .PaddingVertical(2)
                                    .PaddingHorizontal(6)
                                    .Text(cargo.CargoStatus.GetDisplayName())
                                    .FontSize(8).Bold().FontColor(primaryColor);
                            });
                        });

                        headerCol.Item().PaddingTop(12).LineHorizontal(1.5f).LineColor(primaryColor);
                    });
                    page.Content().PaddingTop(15).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor(borderColor).Background(lightBg).Padding(10).Column(c =>
                            {
                                c.Item().Row(r =>
                                {
                                    r.AutoItem().Text("GÖNDERİCİ BİLGİLERİ").FontSize(9).Bold().FontColor(primaryColor);
                                });
                                c.Item().PaddingVertical(4).LineHorizontal(0.5f).LineColor(borderColor);

                                c.Item().Text(cargo.SenderName).FontSize(11).Bold().FontColor("#000000");
                                c.Item().PaddingTop(2).Text($"Çıkış Şubesi: {cargo.OriginBranchName}").FontColor(secondaryColor);
                                c.Item().Text($"Gönderim Tarihi: {cargo.ShipmentDate:dd.MM.yyyy HH:mm}").FontSize(8.5f).FontColor(secondaryColor);
                            });
                            row.Spacing(12);

                            row.RelativeItem().Border(11).BorderColor(borderColor).Background(lightBg).Padding(10).Column(c =>
                            {
                                c.Item().Row(r =>
                                {
                                    r.AutoItem().Text("ALICI BİLGİLERİ").FontSize(9).Bold().FontColor(primaryColor);
                                });
                                c.Item().PaddingVertical(4).LineHorizontal(0.5f).LineColor(borderColor);

                                c.Item().Text(cargo.ReceiverName).FontSize(11).Bold().FontColor("#000000");
                                c.Item().PaddingTop(2).Text($"Varış Şubesi: {cargo.DestinationBranchName}").FontColor(secondaryColor);

                                if (!string.IsNullOrEmpty(cargo.ReceiverPhone))
                                {
                                    c.Item().Text($"Telefon: {cargo.ReceiverPhone}").FontColor(secondaryColor);
                                }

                                c.Item().Text($"Teslimat Adresi: {cargo.DeliveryAddressDetail ?? "-"}").FontSize(8.5f).FontColor(secondaryColor);
                            });
                        });

                        col.Item().PaddingVertical(14);

                        col.Item().Border(1).BorderColor(borderColor).Padding(10).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("AĞIRLIK").FontSize(7.5f).Bold().FontColor(secondaryColor);
                                c.Item().Text($"{cargo.Weight} kg").FontSize(10.5f).Bold();
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("HACİM (DESİ)").FontSize(7.5f).Bold().FontColor(secondaryColor);
                                c.Item().Text($"{cargo.Desi:N2}").FontSize(10.5f).Bold();
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("PAKET TÜRÜ").FontSize(7.5f).Bold().FontColor(secondaryColor);
                                c.Item().Text(cargo.CargoType.GetDisplayName()).FontSize(10.5f).Bold();
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("TAHMİNİ TESLİMAT").FontSize(7.5f).Bold().FontColor(secondaryColor);
                                c.Item().Text($"{cargo.EstimatedArrivalDate:dd.MM.yyyy}").FontSize(10.5f).Bold().FontColor(primaryColor);
                            });
                        });

                        col.Item().PaddingVertical(10);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem(3).Border(1).BorderColor("#a7f3d0").Background("#ecfdf5").Padding(8).Row(r =>
                            {
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("GÜVENLİK TESLİMAT KODU").FontSize(8).Bold().FontColor("#065f46");
                                    c.Item().Text("Paket alıcıya teslim edilirken bu kod personele bildirilmelidir.").FontSize(7.5f).FontColor("#047857");
                                });

                                r.AutoItem().AlignMiddle().Container()
                                .Background("#ffffff")
                                .Border(1)
                                .BorderColor("#6ee7b7")
                                .PaddingVertical(4)
                                .PaddingHorizontal(10)
                                .Text(cargo.DeliveryCode ?? "------")
                                .FontSize(13).FontFamily("Courier New").Black().FontColor("#065f46");

                            });

                            row.Spacing(12);

                            row.RelativeItem(2).Background(primaryColor).Padding(8).Column(c =>
                            {
                                c.Item().Text("TAŞIMA ÜCRETİ").FontSize(8).Bold().FontColor("#94a3b8");
                                c.Item().Text($"{cargo.Price:C2}").FontSize(15).Black().FontColor("#ffffff");
                            });
                        });


                        col.Item().PaddingVertical(15);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Border(0.5f).BorderColor(borderColor).Padding(8).Height(65).Column(c =>
                            {
                                c.Item().Text("Teslim Eden (Kurye / Şube Yetkilisi)").FontSize(8).Bold().FontColor(secondaryColor);
                                c.Item().Text($"Ad Soyad: {cargo.AssignedCourierName ?? "-"}").FontSize(7.5f);
                                c.Item().AlignBottom().Text("İmza: .......................................").FontSize(7.5f).FontColor(secondaryColor);
                            });

                            row.Spacing(12);

                            row.RelativeItem().Border(0.5f).BorderColor(borderColor).Padding(8).Height(65).Column(c =>
                            {
                                c.Item().Text("Teslim Alan (Alıcı)").FontSize(8).Bold().FontColor(secondaryColor);
                                c.Item().Text("Ad Soyad: ....................................................").FontSize(7.5f);
                                c.Item().AlignBottom().Text("İmza: .......................................").FontSize(7.5f).FontColor(secondaryColor);
                            });
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().LineHorizontal(0.5f).LineColor(borderColor);
                        col.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text($"Bu belge CargoTrack otomasyonu üzerinden düzenlenmiştir. | Düzenleme Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(7.5f).FontColor(secondaryColor);
                            row.RelativeItem().AlignRight().Text(x =>
                            {
                                x.Span("Sayfa ");
                                x.CurrentPageNumber();
                                x.Span(" / ");
                                x.TotalPages();
                            });
                        });
                    });
                });
            }).GeneratePdf();
        }
    }
}
