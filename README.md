# Game Room App
Checkpoint 1
- Pentru testare avem in fiserul game-room-app/Postman json-ul pentru teste (versiunea 2.1).
- Modelul aplictatiei se gaseste in DataModel.
- In Provider se repository pentru fiecare model in parte.
- In Controller sunt clasele care se ocupa cu Rest API pentru fiecare model in parte.
- Aplicatia de proneste din clasa Program
- Au fost implementate o parte din functionalitati, restul urmând pe viitor.
- Programul a fost testat pe doua unitati diferite

# 
Checkpoint 2
- se adauga in run comanda: chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
- Proiectul dedicat acestui checkpoint este \game-room-app\game-room-ui
- Programul are ca pagina principala: http://localhost:3000/
- Trebui creat un cont cu care se face logarea
- Principalele functionalitati sunt: 
	- De specificat ca pentru a creat un joc de tipul Darts Cricket sau Darts X01, la type trebuie scris Darts/X01 respectiv Darts/Cricket
	- Crearea de echipe (folositoare in cazul in care dorim sa jucam pe echipe)
	- Afisarea de jocuri terminate si in derulare(in pauza), cele in pauza pot fi reluate. (Se da click pe numele jocului pe care vrem sa-l vedem). 
	- Jocurile terminate afiseaza doar Leaderboardul final.
	- Jocurile neterminate pot fi reluate si finalizate.
	- Bug: Pentru a vedea progresul real cand adaugăm cate un scor/puctaj trebuie reincarcata pagina manual.
- Partea de css nu a fost adaugata încă.
- Pentru alte functionalitati cum ar fi: stergerea de jocuri, stergerea de jucatori... se va adauga in viitor o parte speciala pentru administrator.
- Exista mai multe buguri acestea urmand a fi rezolvate in urmatoarea perioada.
- Fiind prima aplicatie facuta in React lucrurile au mers mai greu.(Am urmărit mai mult functionalitatiile principale).
- Au fost modificate rutele si nu toate au fost actualizate in Postman.


#Checkpoint 3
- Se adauga in run comanda: chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
- Proiectul dedicat acestui checkpoint este \game-room-app\game-room-ui
- Programul are ca pagina principala: http://localhost:3000/
- In partea de inceput este functionala: logarea si inregistrarea. (conditii peste 5 caractere si sa se respecte tiparul de nume)
- Contul ce care se va face logarea este: username: admin123 si password: admin123 
- Dupa ce sa facut logarea avem pagina de profil. (Inca nu am adaugat incarcare a pozei utilizatorului dar o sa fac acest lucru)
- In aceasta parte de profil se pot actualiza datele utilizatorului.
- In navigator avem legaturile la celelalte pagini:
	- Linkul Home ne trimite cate Pagina profilului.
	- Linkul Create Team ne duce catre pagina de creare a echipelor.
		- In inputul de Search for... se adauga jucatori (pe baza username) in echipa (folosindune de butonul Search).
		- Intre acest Form si butonul de save apar jucatorii care au fost adaugati in echipa.
		- Butonul Save insereaza echipa. Numele echipei trebuie sa fie unic, inca un am verificat acest lucru dar o sa urmeze.
	- Butonul Games deschide un meniu in care exista mai multe functionalitati.
		- Linkul Create Game Solo este destinat crearii unui joc 1 vs 1. 
			- Numele jocului nu poate fi empty.(Si trebuie sa fie unic, inca nu am facut aceasta verificare dar urmeaza)
			- In functie de typul jocului exista componente diferite deoarece in cazul darts-ului scorul se calculeaza altfel.
				- Deci exista tipurile: Darts/X01 si Darts/Cricket pentru componente specifice si orice alt tip pentru componente normala.
			- Exact ca la partea de creare a echipei avem de cautat dupa jucatori (pe baza username), pentru a crea un joc avem nevoie mai multi jucatori,
nu se poate cu un singur jucator.
			- Odata ce am cautat jucatorul acesta este introdus in lista de participanti si apare sub forma de card.
			- Butonul Start insereaza jocul (il incepe) redirectionanadune spre pagina jocului.
		- Linkul Create Game Team este destinat crearii unui joc in echipe (din aceasta cauza exista partea de creare a echipei):
			- Totul este exact ca in cazul jocului single player, doar ca in acest caz se aduaga echipe (mai mult de 1) cautarea 
facandu-se pe baza numelui echipei (icon-ul echipei este unul diferit de cel al jucatorului).
		- Unfinish Game List ne duce catre componenta ce contine toate jocurile care nu au fost finalizate.
			- In partea de sus stanga a paginii avem un input si un buton Search. Folosind acestea cautam un joc dupa nume. (Apare doar jocul respectiv)
			- In partea de sus dreapta a paginii avem un buton Filter for the game list care prin apasare ofera o lista de functionalitati: 
				- Lista de jos este modificata in functie de:
					- (Order By the most recent) = Se face ordonare, primele elemente care apar sunt cele mai recente.
					- (Show only DartsX01) = In lista apar numai jocurile de tipul DartsX01.
					- (Show only DartsCricket) = In lista apar numai jocurile de tipul DartsCricket.
					- (Show only Foosball) = In lista apar numai jocurile de tipul Foosball.
					- (Show the rest of Game) = In lista apar jocurile care sunt de tip diferit fata de cele de mai sus.
					- (Reset) = Arata lista normala. (Reseteaza filtrele)
			- In mijlocul paginii avem lista de jocuri, daca se da click pe oricare dintre acestea se deschide pagina specifica lor (In functie de tipurile: 
Darts/Cricket, Darts/X01 sau oricare altul).
			- In chenarul specific jocului exista un checkbox care daca este selectat adauga jocul intro lista de jocuri selectate pentru stergere, aceasta 
se face prin apasarea butonului DeleteGames.
			- Prin butonul next se afiseaza urmatoarele jocuri cate 4 pe pagina (se poate modifica numarul de elemente dar din lipsa de elemente s-a ales 4).
			- Prin butonul back se afiseaza 4 jocuri din urma.
			- Prin aceasta s-a facut paginarea, de mentionat ca merge si la filtrari.
		- Finish Game List ne duce catre componenta ce contine toate jocurile care nu au fost finalizate.
			- Sunt aceleasi functionalitati ca mai sus.
		-! De specificat ca jocurile din liste sunt ale utilizatorului curent.
	- Pentru a putea vizualiza paginile destinate jocurilor trebuie sa creati niste jocuri doarece apar niste erori legate de id-uri in cazul jocurilor generate.
	- Pentru a usura munca (in crearea de jocuri) - la jocurile solo avem jucatorii: andreiM, mAndrea, noritsRad, gicuggg, mitaurel, admin123 .(Acestea sunt username-uri)
						      - la jocurile multi avem echipele: Fire, Air, Phonex. 
							(Deoarece pentru a putea vizualiza jocul in listele de jocuri, acesta trebuie sa apartina in joc trebuie ca Echipa Phonex 
(admin123 este in ea)) sa fie folosita sau in cazul solo el (admin123) trebuie adaugat in lista de participanti.
	
	- Pagina specifica jocurilor de un tip oarecare are urmatoarele functionalitati:
		* In stanga:
			- Adaugarea de puncte 
			- Clasamentul
			- Butonul de finish (folosit pentru a marca jocul ca finalizat)
		* In dreapta 
			- Adaugarea de momente de victorie si de jena (infrangere)
			- Aceste imagini apar mai jos in doua galerii
			- Pentru A vedea componenta de IA trebuie dat click pe poza pe care dorim sa o analizam.
			! La final voi prezenta partea de IA
	- Pagina specifica jocurilor de un tip Darts/X01 are urmatoarele functionalitati:
		
		* In stanga:
			- Alegerea tipului de X01 (301, 501, 1001) adica de la cate puncte se incepe. By default se incepe de la 501.
			- Adaugarea de puncte 
			- Clasamentul
			- Butonul de finish (folosit pentru a marca jocul ca finalizat)
		* In dreapta 
			- Adaugarea de momente de victorie si de jena (infrangere). 
			- Aceste imagini apar mai jos in doua galerii
			- Pentru A vedea componenta de IA trebuie dat click pe poza pe care dorim sa o analizam.
			! La final voi prezenta partea de IA

	- Pagina specifica jocurilor de un tip Darts/Cricket are urmatoarele functionalitati:

		* In stanga:
			- Adaugarea de puncte (activarea si blocarea) .
			- Am considerat ca deoarece este un joc de doua persoane in genral, nu este nevoie de un clasament.
			- Butonul de finish (folosit pentru a marca jocul ca finalizat)
		* In dreapta 
			- Adaugarea de momente de victorie si de jena (infrangere). 
			- Aceste imagini apar mai jos in doua galerii
			- Pentru A vedea componenta de IA trebuie dat click pe poza pe care dorim sa o analizam.
			! La final voi prezenta partea de IA

	- Pantru jocurile in echipa avem fix aceleasi pagini.

	Partea de inteligenta artificiala:
		- Dam click pe imagine si ajunge la alta pagina.
		- In aceasta pagina avem imaginea pe care am dat click si un buton Click to find what game you are playing!
		- La apasarea acestui buton in cateva secunde obtine o predicte in privinta jocului pe care il jucam.
		- Jocurile pentru care a fost atrenat sunt: darts, foosball si fifa. Cu cate 30 de poze la fiecare. Deci trebuie folosite poze specifice.
		- Legatura se face in server unde avem in folderul Util doua module care se ocupa cu acest lucru.
		- O sa adaug cu timpul mai multe categorii de poze cu jocuri. Acest exemplu serveste mai mult pentru a arata cum se face legatura 
si extragerea de date intre Custom Vison AI si aplicatia noastra.
	- In header mai avem butonul de logout care elimina utilizatorul din local storage.
- In Footer avem about si Terms and Conditions, acestea nu sunt implementate inca.

- Nu am implementat inca partea de social media integration si conectarea pe sesiune.

	 

		
					

	


