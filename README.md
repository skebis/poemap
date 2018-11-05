Tietotekniikan ohjelmointityö, Emil Keränen, 5.11.2018
Aihe/nimi: Yksinkertainen osto-ohjelma kartoille Path of Exile -videopeliä varten

Ohjelmointityön tarkoituksena on tehdä web sovellus karttojen ostamiseen Path of Exilessä. Pelin kehittäjät ovat antaneet yhteisön käyttöön Public stash tab API:n, joka antaa lähes reaaliajassa tietoa pelaajien julkisten "stash tabien" sisällön ja tietoa niistä. Uusin tieto haetaan JSON muodossa sivulta (https://poe.ninja/stats). Esimerkiksi tätä ohjelmaa varten tarvitaan myynnissä olevien karttojen tiedot (mikä kartta kyseessä, myyjän nimi, hinta..). JSONin rakenne löytyy sivulta (https://pathofexile.gamepedia.com/Public_stash_tab_API).

Työn haasteena tulee olemaan järkevä ja nopea JSON-tiedon purkaminen, jotta ostamisesta tulee varmasti nopeaa ja todella yksinkertaista. Käyttäjän ei tarvitse selata monien eri nappien ja listojen väliltä, vaan tarkoitus olisi tehdä tehokas etsimisfunktio, joka toimisi nopeasti jo parilla syötteellä. Myös hinnan mukaan lajittelu tulee olemaan haastavaa, koska valuuttana toimii eräänlaiset "orbit", joiden arvojen vaihtelu on lähes täysin pelaajien hallussa. Arvot löytyvät kuitenkin reaaliajassa myös sivulta (https://poe.ninja).

Tärkeimmät työkalut tulevat olemaan todennäköisesti Javascript, HTML ja CSS. Myös C# ja laajemmin ASP.NET voisivat olla hyvä idea, mutta niiden opetteluun voi mennä aikaa. Työ olisi tarkoitus toteuttaa 2018 loppusyksyn ja 2019 alkukevään aikana, karkeana arviona 10/2018 ja 1/2019 välisenä aikana.

Lisätietoa saa myös sivulta (https://www.reddit.com/r/pathofexiledev/comments/48i4s1/information_on_the_new_stash_tab_api/).

(Miksi juuri karttojen ostamiseen tarkoitettu ohjelma? Kartat (maps) ovat oleellisin osa Path of Exilen loppupeliä. Lähes kaikki toiminta tapahtuu kartoissa ja tätä varten on myös Atlas, joka kertoo mm. mitkä kartat on suoritettu. Erilaisia karttoja on n. 150 ja Atlaksen täydellinen suoritus lisää hyvien ja/tai kalliiden karttojen tippumismahdollisuutta (drop rate) ja on monille pelaajille päätavoite pelin "läpipelaamiseen". Jokaisen kartan saaminen itse on tosin hyvin epätodennäköistä, joten monet pelaajat joutuvat ostamaan karttoja muilta pelaajilta. Yleistä tavaroiden myymistä varten on tehty jo monia sivuja, kuten (poe.trade), mutta yksinkertaisia, pelkkien karttojen ostamista varten olevia sivuja ei mielestäni ole.)
