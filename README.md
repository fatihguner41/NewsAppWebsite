
# NewsApp Website
## Proje açıklaması:
  Bu projeyi gerçekleştirme amacım ".net" ve "asp.net" teknolojilerini öğrenmektir. Herhangi bir ticari amacı yoktur.Önceden geliştirmiş olduğum ve newsapp uygulaması için hazırlanmış olan firebase veri tabanı bu website projesine de dahil edilmiştir ve mobil uygulamanın website versiyonu asp.net teknolojisi kullanılarak tasarlanmıştır. Firebase veri tabanına buluttaki sunucumuzdan her 5 dakikada bir yeni haberler "web scrapping" yöntemi ile çekilmekte ve eklenmekte. Böylece bize website backendinde sadece bu haberleri tarihe ve istenilen kategorilere göre sıralayarak kullanıcılarla buluşturmak kalıyor. Yeni haberleri getirme fonksiyonu herhangi bir butonla veya sayfalama yöntemiyle değil de sayfanın sonuna ulaşma olayı ile tetiklenir. Bu daha kolay bi kullanım sağlar. Yani sayfanın sonuna geldiğinizde otomatik olarak yeni haberler yüklenecektir. Sayfa tasarımı bootstrap kütüphanesi ile yapılmıştır.
## özellikler:
### Kenar menüsü:
  Kenar menüsünde haberlerin kategorilerine göre veya yayınlandığı gazetelere göre özelleştirilmiş arama yapmamızı sağlayan bir tasarım bulunmakta. Tam 6 haber kategorisi ve 20 kaynak gazete içinden istediğiniz gibi seçim yapabilir ve o alanla ilgili haberleri görüntüleyebilirsiniz.

### Haber kartları:
  Her bir haber kendisine ait bir haber kartı içinde görselleştirilir. Bu kartın içinde haberin resmi, başlığı, paylaşıldığı gazete ve paylaşılma tarihi yer alır. Haber kartına tıklanıldığında ise haberin paylaşıldığı siteye yönlendirilirsiniz.

### Arama kutusu:
  Sayfanın en üstünde yer alan arama kutusuna herhangi bir konu girebilir ve dilediğiniz konuda en güncel haberleri inceleyebilirsiniz.

# English
## Project Description

The primary goal of this project is to gain proficiency in ".NET" and "ASP.NET" technologies. It is not intended for commercial purposes. The Firebase database, previously prepared for the "NewsApp" mobile application, is integrated into this web project. The web version of the mobile application is crafted using the "ASP.NET" technology.

New articles are pulled and added to the Firebase database from our cloud server every 5 minutes using the "web scraping" method. Consequently, in the web site's backend, the only responsibility is presenting these articles to users, sorted by date and preferred categories. The function to fetch new articles is activated by reaching the end of the page, ensuring a seamless user experience. As you reach the end of the page, new articles are loaded automatically. The page design is implemented using the "Bootstrap" library.

## Features

### Sidebar
- Customized searches based on news categories or the newspapers they are published in.
- Choose from 6 news categories and 20 source newspapers to view news related to the selected area.

### News Cards
- Each news article is visualized within its own news card.
- The card includes the article's image, title, the newspaper it was published from, and the publish date.
- Clicking on a news card redirects you to the site where the news is published.

### Search Box
- Enter any topic into the search box at the top of the page to explore the latest news on the desired subject.
