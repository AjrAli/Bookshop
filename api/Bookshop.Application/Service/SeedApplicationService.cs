using Bookshop.Application.Contracts.Seed;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Service
{
    public class SeedApplicationService : ISeedApplicationService
    {
        private readonly BookshopDbContext _dbContext;

        public SeedApplicationService(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SeedInfrastructureDataAsync()
        {
            await SeedLocationPricingsAsync();
            await SeedBooksAsync();
        }


        public async Task SeedLocationPricingsAsync()
        {
            if (!await _dbContext.LocationPricings.AnyAsync())
            {
                var listLocationPricing = new List<LocationPricing>
                {
                    new LocationPricing("Belgium", 10m, 21m),
                    new LocationPricing("Spain", 14.99m, 18m),
                    new LocationPricing("United Kingdom", 19.99m, 11m),
                    new LocationPricing("France", 4.99m, 12m),
                    new LocationPricing("Germany", 5.99m, 15m),
                    new LocationPricing("Italy", 19.99m, 19m),
                    new LocationPricing("OTHERS", 19.99m, 21m)
                };
                await _dbContext.LocationPricings.AddRangeAsync(listLocationPricing);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task SeedBooksAsync()
        {
            if (!await _dbContext.Books.AnyAsync())
            {
                var author1 = new Author("Aghata Cristie", @"Dame Agatha Mary Clarissa Christie, Lady Mallowan,
                                                          DBE (née Miller; 15 September 1890 – 12 January 1976) was an English writer known
                                                          for her 66 detective novels and 14 short story collections, particularly those revolving
                                                          around fictional detectives Hercule Poirot and Miss Marple.");
                var cat1 = new Category("Detective Stories", @"Detective fiction is a subgenre of crime fiction and mystery fiction
                                                               in which an investigator or a detective—whether professional, amateur 
                                                               or retired—investigates a crime, often murder.");
                var author2 = new Author("Louis Sachar", @"Louis Sachar is the author of the #1 New York Times bestseller Holes, which won the Newbery Medal, the National Book Award, and the Christopher Award, as well as Stanley Yelnats' Survival to Camp Green Lake; Small Steps, winner of the Schneider Family Book Award; and The Cardturner, a Publishers Weekly Best Book, a Parents' Choice Gold Award recipient, and an ALA-YALSA Best Fiction for Young Adults Book. His books for younger readers include There's a Boy in the Girls' Bathroom, The Boy Who Lost His Face, Dogs Don't Tell Jokes, and the Marvin Redpost series, among many others.");
                var cat2 = new Category("Thriller", @"Thriller is a genre of fiction with numerous, often overlapping, subgenres, including crime, horror, and detective fiction. Thrillers are characterized and defined by the moods they elicit, giving their audiences heightened feelings of suspense, excitement, surprise, anticipation and anxiety.[1] This genre is well suited to film and television.");
                var author3 = new Author("David Grann", @"DAVID GRANN is the author of the #1 New York Times bestsellers KILLERS OF THE FLOWER MOON and THE LOST CITY OF Z. KILLERS OF THE FLOWER MOON was a finalist for the National Book Award and won an Edgar Allan Poe Award. He is also the author of THE WHITE DARKNESS and the collection THE DEVIL AND SHERLOCK HOLMES. Grann's investigative reporting has garnered several honors, including a George Polk Award. He lives with his wife and children in New York.");
                var author4 = new Author("Rebecca Yarros", @"Rebecca Yarros is the #1 New York Times, USA Today, and Wall Street Journal bestselling author of over fifteen novels including Fourth Wing and In the Likely Event, with multiple starred Publishers Weekly reviews and a Kirkus Best Book of the Year. She loves military heroes and has been blissfully married to hers for over twenty years. She's the mother of six children, and is currently surviving the teenage years with two of her four hockey-playing sons. When she's not writing, you can find her at the hockey rink or sneaking in some guitar time while guzzling coffee. She and her family live in Colorado with their stubborn English bulldogs, two feisty chinchillas, and a Maine Coon cat named Artemis, who rules them all. ");
                var cat3 = new Category("Fiction", @"Fiction is any creative work, chiefly any narrative work, portraying individuals, events, or places that are imaginary or in ways that are imaginary.Fictional portrayals are thus inconsistent with history, fact, or plausibility. In a traditional narrow sense, fiction refers to written narratives in prose – often referring specifically to novels, novellas, and short stories.More broadly, however, fiction encompasses imaginary narratives expressed in any medium, including not just writings but also live theatrical performances, films, television programs, radio dramas, comics, role-playing games, and video games.");
                var author5 = new Author("Martha Wells", @"MARTHA WELLS has written many novels, including the million-selling New York Times and USA Today-bestselling Murderbot Diaries series, which has won multiple Hugo, Nebula, Locus, and Alex Awards. Other titles include Witch King, City of Bones, The Wizard Hunters, Wheel of the Infinite, the Books of the Raksura series (beginning with The Cloud Roads and ending with The Harbors of the Sun), and the Nebula-nominated The Death of the Necromancer, as well as YA fantasy novels, short stories, and nonfict");
                var author6 = new Author("Joe Schreiber", @"Joe Schreiber is the author of the Star Wars novels Maul: Lockdown, Death Troopers, and Red Harvest, as well as the junior novel for Solo: A Star Wars Story. His original work includes Chasing the Dead, Eat the Dark, Au Revoir, Crazy European Chick, and Game Over, Pete Watson . He was born in Michigan but spent his formative years in Alaska, Wyoming, and Northern California. He lives in Cotati, CA with his wife, two children, and several original Star Wars action figures.");
                var cat4 = new Category("Comics", @"A comic book, also called comicbook,comic magazine or simply comic, is a publication that consists of comics art in the form of sequential juxtaposed panels that represent individual scenes. Panels are often accompanied by descriptive prose and written narrative, usually, dialogue contained in word balloons emblematic of the comics art form.");
                var listBooks = new List<Book>
                {

                    new Book("EL Misterio Del Tren azul",
                             @"Cuando el lujoso Tren Azul llega a Niza, 
                              un guardia intenta despertar a Ruth Kettering para anunciarle su parada. Pero ella no despertar nunca m s, 
                              ya que un disparo de gran calibre la ha matado, desfigurando sus rasgos hasta volverla casi irreconocible. 
                              Adem s, sus valios simos rub es han desaparecido. El principal sospechoso del crimen es el arruinado marido de la dama, Derek. 
                              Pero Poirot no est convencido, y decide hacer una reconstrucci n de ese d a hasta llegar a la clave del asesinato... ",
                             "Createspace Independent Publishing Platform",
                             "9781535317634",
                             12.3m,
                             1000,
                             354,
                            "7.99 X 0.6 X 10.0 inches | 1.25 pounds",
                            "/client/img/9781535317634.jpg",
                            Book.Languages.Spanish,
                            new DateTime(2016, 7, 16, 7, 0, 0),
                            author1,
                            cat1),
                    new Book("Holes",
                             @"This groundbreaking classic is now available in a special anniversary edition with bonus content. 
							 Winner of the Newbery Medal as well as the National Book Award, HOLES is a New York Times bestseller
							 and one of the strongest-selling middle-grade books to ever hit shelves!Stanley Yelnats is under a curse. 
							 A curse that began with his no-good-dirty-rotten-pig-stealing-great-great-grandfather and has since followed
							 generations of Yelnatses. Now Stanley has been unjustly sent to a boys' detention center, Camp Green Lake,
							 where the boys build character by spending all day, every day digging holes exactly five feet wide and five
							 feet deep. There is no lake at Camp Green Lake. But there are an awful lot of holes.It doesn't take long for
							 Stanley to realize there's more than character improvement going on at Camp Green Lake. The boys are digging
							 holes because the warden is looking for something. But what could be buried under a dried-up lake? Stanley
							 tries to dig up the truth in this inventive and darkly humorous tale of crime and punishment--and redemption.
							 Special anniversary edition bonus content includes: A New Note From the Author! Ten Things You May Not Know
							 About HOLES by Louis Sachar and more!",
                             "Yearling Books",
                             "9780440414803",
                             8.36m,
                             1000,
                             288,
                            "5.2 X 7.5 X 0.8 inches | 0.42 pounds",
                            "/client/img/9780440414803.jpg",
                            Book.Languages.English,
                            new DateTime(2000, 05, 09, 7, 0, 0),
                            author2,
                            cat1),
                    new Book("The Wager: A Tale of Shipwreck, Mutiny and Murder",
                             @"On January 28, 1742, a ramshackle vessel of patched-together wood and cloth washed up on the coast of Brazil. Inside were thirty emaciated men, barely alive, and they had an extraordinary tale to tell. They were survivors of His Majesty's Ship the Wager, a British vessel that had left England in 1740 on a secret mission during an imperial war with Spain. While the Wager had been chasing a Spanish treasure-filled galleon known as the prize of all the oceans, it had wrecked on a desolate island off the coast of Patagonia. The men, after being marooned for months and facing starvation, built the flimsy craft and sailed for more than a hundred days, traversing nearly 3,000 miles of storm-wracked seas. They were greeted as heroes.",
                             "Doubleday Books",
                             "9780385534260",
                             27.90m,
                             1000,
                             352,
                            "6.38 X 9.51 X 1.42 inches | 1.5 pounds",
                            "/client/img/9780385534260.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 04, 18, 7, 0, 0),
                            author3,
                            cat2),
                    new Book("Iron Flame",
                             @"Everyone expected Violet Sorrengail to die during her first year at Basgiath War College--Violet included. But Threshing was only the first impossible test meant to weed out the weak-willed, the unworthy, and the unlucky.Now the real training begins, and Violet's already wondering how she'll get through. It's not just that it's grueling and maliciously brutal, or even that it's designed to stretch the riders' capacity for pain beyond endurance. It's the new vice commandant, who's made it his personal mission to teach Violet exactly how powerless she is-unless she betrays the man she loves. ",
                             "Entangled: Red Tower Books",
                             "9781649374172",
                             27.89m,
                             1000,
                             640,
                            "6.5 X 8.5 X 2.8 inches | 1.7 pounds",
                            "/client/img/9781649374172.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 11, 07, 7, 0, 0),
                            author4,
                            cat3),
                    new Book("System Collapse",
                             @"Am I making it worse? I think I'm making it worse.Following the events in Network Effect, the Barish-Estranza corporation has sent rescue ships to a newly-colonized planet in peril, as well as additional SecUnits. But if there's an ethical corporation out there, Murderbot has yet to find it, and if Barish-Estranza can't have the planet, they're sure as hell not leaving without something. If that something just happens to be an entire colony of humans, well, a free workforce is a decent runner-up prize. ",
                             "Tordotcom",
                             "9781250826978",
                             20.45m,
                             1000,
                             256,
                            "5.35 X 8.3 X 0.69 inches | 0.67 pounds",
                            "/client/img/9781250826978.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 11, 14, 7, 0, 0),
                            author5,
                            cat3),
                    new Book("Star Wars: The Mandalorian Junior Novel",
                             @"After the fall of the Empire but before the emergence of the First Order, a lone bounty hunter known as The Mandalorian travels the outer reaches of the galaxy. When his newest bounty hunting target turns out to be a small Child, the Mandalorian decides the Child must be protected at all costs. Relive all the excitement of the first season of the smash-hit streaming series in this action-packed junior novel by Joe Schreiber! Includes an insert of color photos from the show! ",
                             "Disney Lucasfilm Press",
                             "9781368057134",
                             6.50m,
                             1000,
                             208,
                            "5.2 X 7.5 X 0.7 inches | 0.35 pounds",
                            "/client/img/9781368057134.jpg",
                            Book.Languages.English,
                            new DateTime(2021, 01, 05, 7, 0, 0),
                            author6,
                            cat3),
                   new Book("Star Wars: Ahsoka",
                             @"Finally, her story will begin to be told. Following her experiences with the Jedi and the devastation of Order 66, Ahsoka is unsure she can be part of a larger whole ever again. But her desire to fight the evils of the Empire and protect those who need it will lead her right to Bail Organa, and the Rebel Alliance...",
                             "Disney Lucasfilm Press",
                             "9781484782316",
                             6.50m,
                             1000,
                             208,
                            "5.2 X 7.5 X 0.7 inches | 0.35 pounds",
                            "/client/img/9781484782316.jpg",
                            Book.Languages.English,
                            new DateTime(2021, 01, 05, 7, 0, 0),
                            author6,
                            cat3),
                    new Book("Parachute Kids: A Graphic Novel",
                             @"Feng-Li can't wait to discover America with her family! But after an action-packed vacation, her parents deliver shocking news: They are returning to Taiwan and leaving Feng-Li and her older siblings in California on their own.",
                             "Graphix",
                             "9781338832686",
                             12.08m,
                             1000,
                             288,
                            "5.51 X 7.95 X 1.02 inches | 1.36 pounds",
                            "/client/img/9781338832686.jpg",
                            Book.Languages.English,
                            new DateTime(2022, 02, 05, 7, 0, 0),
                            author6,
                            cat4),
                    new Book("EL Misterio Del Tren azul 2",
                             @"Cuando el lujoso Tren Azul llega a Niza, 
                              un guardia intenta despertar a Ruth Kettering para anunciarle su parada. Pero ella no despertar nunca m s, 
                              ya que un disparo de gran calibre la ha matado, desfigurando sus rasgos hasta volverla casi irreconocible. 
                              Adem s, sus valios simos rub es han desaparecido. El principal sospechoso del crimen es el arruinado marido de la dama, Derek. 
                              Pero Poirot no est convencido, y decide hacer una reconstrucci n de ese d a hasta llegar a la clave del asesinato... ",
                             "Createspace Independent Publishing Platform",
                             "9781535317634",
                             12.3m,
                             1000,
                             354,
                            "7.99 X 0.6 X 10.0 inches | 1.25 pounds",
                            "/client/img/9781535317634.jpg",
                            Book.Languages.Spanish,
                            new DateTime(2016, 7, 16, 7, 0, 0),
                            author1,
                            cat1),
                    new Book("Holes 2",
                             @"This groundbreaking classic is now available in a special anniversary edition with bonus content. 
							 Winner of the Newbery Medal as well as the National Book Award, HOLES is a New York Times bestseller
							 and one of the strongest-selling middle-grade books to ever hit shelves!Stanley Yelnats is under a curse. 
							 A curse that began with his no-good-dirty-rotten-pig-stealing-great-great-grandfather and has since followed
							 generations of Yelnatses. Now Stanley has been unjustly sent to a boys' detention center, Camp Green Lake,
							 where the boys build character by spending all day, every day digging holes exactly five feet wide and five
							 feet deep. There is no lake at Camp Green Lake. But there are an awful lot of holes.It doesn't take long for
							 Stanley to realize there's more than character improvement going on at Camp Green Lake. The boys are digging
							 holes because the warden is looking for something. But what could be buried under a dried-up lake? Stanley
							 tries to dig up the truth in this inventive and darkly humorous tale of crime and punishment--and redemption.
							 Special anniversary edition bonus content includes: A New Note From the Author! Ten Things You May Not Know
							 About HOLES by Louis Sachar and more!",
                             "Yearling Books",
                             "9780440414803",
                             8.36m,
                             1000,
                             288,
                            "5.2 X 7.5 X 0.8 inches | 0.42 pounds",
                            "/client/img/9780440414803.jpg",
                            Book.Languages.English,
                            new DateTime(2000, 05, 09, 7, 0, 0),
                            author2,
                            cat1),
                    new Book("The Wager: A Tale of Shipwreck, Mutiny and Murder 2",
                             @"On January 28, 1742, a ramshackle vessel of patched-together wood and cloth washed up on the coast of Brazil. Inside were thirty emaciated men, barely alive, and they had an extraordinary tale to tell. They were survivors of His Majesty's Ship the Wager, a British vessel that had left England in 1740 on a secret mission during an imperial war with Spain. While the Wager had been chasing a Spanish treasure-filled galleon known as the prize of all the oceans, it had wrecked on a desolate island off the coast of Patagonia. The men, after being marooned for months and facing starvation, built the flimsy craft and sailed for more than a hundred days, traversing nearly 3,000 miles of storm-wracked seas. They were greeted as heroes.",
                             "Doubleday Books",
                             "9780385534260",
                             27.90m,
                             1000,
                             352,
                            "6.38 X 9.51 X 1.42 inches | 1.5 pounds",
                            "/client/img/9780385534260.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 04, 18, 7, 0, 0),
                            author3,
                            cat2),
                    new Book("Iron Flame 2",
                             @"Everyone expected Violet Sorrengail to die during her first year at Basgiath War College--Violet included. But Threshing was only the first impossible test meant to weed out the weak-willed, the unworthy, and the unlucky.Now the real training begins, and Violet's already wondering how she'll get through. It's not just that it's grueling and maliciously brutal, or even that it's designed to stretch the riders' capacity for pain beyond endurance. It's the new vice commandant, who's made it his personal mission to teach Violet exactly how powerless she is-unless she betrays the man she loves. ",
                             "Entangled: Red Tower Books",
                             "9781649374172",
                             27.89m,
                             1000,
                             640,
                            "6.5 X 8.5 X 2.8 inches | 1.7 pounds",
                            "/client/img/9781649374172.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 11, 07, 7, 0, 0),
                            author4,
                            cat3),
                    new Book("System Collapse 2",
                             @"Am I making it worse? I think I'm making it worse.Following the events in Network Effect, the Barish-Estranza corporation has sent rescue ships to a newly-colonized planet in peril, as well as additional SecUnits. But if there's an ethical corporation out there, Murderbot has yet to find it, and if Barish-Estranza can't have the planet, they're sure as hell not leaving without something. If that something just happens to be an entire colony of humans, well, a free workforce is a decent runner-up prize. ",
                             "Tordotcom",
                             "9781250826978",
                             20.45m,
                             1000,
                             256,
                            "5.35 X 8.3 X 0.69 inches | 0.67 pounds",
                            "/client/img/9781250826978.jpg",
                            Book.Languages.English,
                            new DateTime(2023, 11, 14, 7, 0, 0),
                            author5,
                            cat3),
                    new Book("Star Wars: The Mandalorian Junior Novel 2",
                             @"After the fall of the Empire but before the emergence of the First Order, a lone bounty hunter known as The Mandalorian travels the outer reaches of the galaxy. When his newest bounty hunting target turns out to be a small Child, the Mandalorian decides the Child must be protected at all costs. Relive all the excitement of the first season of the smash-hit streaming series in this action-packed junior novel by Joe Schreiber! Includes an insert of color photos from the show! ",
                             "Disney Lucasfilm Press",
                             "9781368057134",
                             6.50m,
                             1000,
                             208,
                            "5.2 X 7.5 X 0.7 inches | 0.35 pounds",
                            "/client/img/9781368057134.jpg",
                            Book.Languages.English,
                            new DateTime(2021, 01, 05, 7, 0, 0),
                            author6,
                            cat3),
                   new Book("Star Wars: Ahsoka 2",
                             @"Finally, her story will begin to be told. Following her experiences with the Jedi and the devastation of Order 66, Ahsoka is unsure she can be part of a larger whole ever again. But her desire to fight the evils of the Empire and protect those who need it will lead her right to Bail Organa, and the Rebel Alliance...",
                             "Disney Lucasfilm Press",
                             "9781484782316",
                             6.50m,
                             1000,
                             208,
                            "5.2 X 7.5 X 0.7 inches | 0.35 pounds",
                            "/client/img/9781484782316.jpg",
                            Book.Languages.English,
                            new DateTime(2021, 01, 05, 7, 0, 0),
                            author6,
                            cat3),
                    new Book("Parachute Kids: A Graphic Novel 2",
                             @"Feng-Li can't wait to discover America with her family! But after an action-packed vacation, her parents deliver shocking news: They are returning to Taiwan and leaving Feng-Li and her older siblings in California on their own.",
                             "Graphix",
                             "9781338832686",
                             12.08m,
                             1000,
                             288,
                            "5.51 X 7.95 X 1.02 inches | 1.36 pounds",
                            "/client/img/9781338832686.jpg",
                            Book.Languages.English,
                            new DateTime(2022, 02, 05, 7, 0, 0),
                            author6,
                            cat4)

                };
                await _dbContext.Books.AddRangeAsync(listBooks);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
