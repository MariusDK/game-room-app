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