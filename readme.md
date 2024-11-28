# DataDefenders WebGoat Analyse
Velkommen til vores semesterprojekt i faget Software Sikkerhed på uddannelsen IT-sikkerhed ved UCL Seebladsgade i Odense. 

Datadefenders er:  
- John Kiærbye Lange - jkla40320@edu.ucl.dk
- Niels Peter Frederiksen - npfr49165@edu.ucl.dk
- Benny Nielsen – bbni49212@edu.ucl.dk

Dette projekt er en fork af vores undervisers projekt WebGoat: https://github.com/mesn1985/WebGoat.   Vores arbejde med at forbedre projektet er dokumenteret i rapporten "Sikkerhed i WebGoat.NET.pdf", som kan findes i dette repository

---

### Problemformulering: 

Hvordan kan vi øge sikkerheden i webapplikationen WebGoat?

---

### Abstract / Resumé

Formålet med projektet var at identificere, evaluere og afhjælpe sårbarheder gennem anvendelse af risikostyring, trusselsmodellering og automatiserede værktøjer som CodeQL, SNYK og Dependabot.
Analysen identificerede kritiske sårbarheder som SQL-injektion, cross-site scripting (XSS) og usikre tredjepartsafhængigheder. 

For at mitigere disse blev der implementeret løsninger som HTTPS-kryptering, HSTS, parametriserede inputs og stærk inputvalidering via domæne primitiver og invarians.  Implementeringen blev valideret gennem unit testing og en CI/CD-pipeline, hvilket sikrede kontinuerlig kvalitet og stabilitet.
Projektet demonstrerer, hvordan strukturerede metoder og moderne sikkerhedsprincipper som "Secure By Design" kan anvendes til at styrke sikkerheden i webapplikationer.   

På trods af projektets begrænsninger har resultaterne bidraget til en væsentlig forbedring af WebGoats sikkerhedsniveau og lagt fundamentet for yderligere udvikling og vedligeholdelse.


---


### Systemkrav
Projektet kræver `.NET 8.0 SDK`

---

### Start applikationen
Kør kommandoen `dotnet run` i mappen for projektet `WebGoat.NET`.

---

### Nulstil database
Databasen, der bruges, gemmes i filen NORTHWND.sqlite. For at nulstille databasen skal du blot erstatte filen, med den fra dette repository.

---
