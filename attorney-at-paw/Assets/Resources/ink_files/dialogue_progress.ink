VAR ultimate_1_failer = false
VAR treatNum = 0
VAR mouseNum = 0
VAR birdNum = 0
VAR yarnNum = 0
VAR petNum = 0

VAR treatIdeal = 6
VAR mouseIdeal = 10
VAR birdIdeal = 3
VAR yarnIdeal = 0 //y'know what, exclusively yarn
VAR petIdeal = 1

VAR treatStr = "TREAT"
VAR mouseStr = "MOUSE"
VAR birdStr = "BIRD"
VAR yarnStr = "YARN"
VAR petStr = "PET"

VAR currentOffset = 546
VAR needMoreStr = "REVOLUTION"
VAR leeway = 45 // 3 off in each category
VAR victory = false

//real file
PRESS SPACE OR RETURN TO BEGIN GAME
>>> START_DIALOGUE
>>> CUTSCENE:OPEN_1
On a warm summer evening, as the sun vanished into the horizon of the Catskill Delta, the western half of Pawnnsylvania began to settle down.
>>> CUTSCENE:OPEN_2
As biscuit makers ceased their operations and couriers delivered the day’s last rats and twigs, one could sense the whole region settling into a catnap.
>>> CUTSCENE:OPEN_3
In the city of Kittsburgh, a young couple and a veterinarian remained awake, all three in the city’s general Pawspital. 
>>> CUTSCENE:OPEN_4
There were a total of three cats awake in the city. 
>>> CUTSCENE:OPEN_5
Then four.
>>> CUTSCENE: OPEN_6
Late into the night, a young kitten was born. So did Paldo, eldest of his litter, come into this world.
>>> CUTSCENE: OPEN_7
Paldo grew up happy in Kittsburgh.
>>> CUTSCENE: OPEN_8
He was a prodigy in scratching post, and his skill at worm-on-a-string was unmatched among his friends.
>>> CUTSCENE: OPEN_9
As his family grew, his apartment became a humble wonder of the world. A home in which any kitty could live and grow up to be the best they can be.
>>> CUTSCENE: OPEN_10
Paldo was restless, only sleeping for 12 hours a day, and graduated high school at the top of his class.
>>> CUTSCENE: OPEN_11
And with his unbreakable drive, nokitty was surprised when he was admitted to the Kittsburgh College of Law. He became the star of the Pawlitical Science department. 
>>> CUTSCENE: OPEN_12
But one night, after a catnip-filled college party, he got a call on his phone.
>>> CUTSCENE: OPEN_13
It was mama. 
>>> CUTSCENE: OPEN_14
She said he had received a notice. “A new highway is scheduled for construction next year”, it read. “And your apartment is in its planned route.” 
>>> CUTSCENE: OPEN_15
It listed, she explained, a series of options for relocation, assuring her that all necessary expenses would be paid for.
>>> CUTSCENE: OPEN_16
But it couldn’t support the size of her brood, or even half of it.
>>> CUTSCENE: OPEN_17
NO_SPEAKER:Before she found the words to ask, Paldo spoke: “I’ll go to Felinedelphia and make this right.
>>> CUTSCENE: OPEN_18
I’ll contact my local congressperson.”
>>> CUTSCENE: TCARD_1
Part 1...
>>> PUT:CHAIRCAT,L
>>> PUT:LEGISLATOR1,R
>>> CUTSCENE:CLEAR
>>> MV:L
CHAIRCAT: Welcome to the thirtieth meeting of the Felinedelphia Catsoviet!
CHAIRCAT: We begin, in accordance with the first article of the Constitution of the Pawnsylvanian Union of Revolutionary Republics (PURR), with an Open Forum, 
CHAIRCAT: for the cats of Pawnsylvania to make their grievances known to the governing body.
CHAIRCAT: Each cat will have one minute to speak before the Soviet, and we will proceed from speaker to speaker in alphabetical order.
CHAIRCAT: I remind the legislators that the Constitution of PURR requires them to account for the grievances of those who speak before them, 
CHAIRCAT: or face removal.
>>> MV:R
LEGISLATOR1: *GASP!*
>>> MV:L
CHAIRCAT: All of you already know this; you have no reason to gasp. 
CHAIRCAT: At any rate, we begin the airing of grievances with our first speaker, AAAAAAAA AAAAAAAAAAAAAAAAAAAA.
>>> PUT:AAA,L
AAA: Good afternoon, kitties of the chamber. 
>>> MV:R
LEGISLATOR1:  Is your name actually AAAAAAAA AAAAAAAAAAAAAAAAAAAA?
>>> MV:L
AAA: I changed it because I wanted to be first on alphabetized lists. 
>>> MV:R
LEGISLATOR1: That's stupid.
>>> MV:L
AAA: How dare you say that to me?! I’m the star of the phonebook!
>>> MV:R
LEGISLATOR1: ...
>>> MV:L
AAA: The captain of the census!
>>> MV:R
LEGISLATOR1: ...
>>> MV:L
AAA: ...
>>> PUT:CHAIRCAT,R
>>> MV:R
CHAIRCAT: Let the record show that that is stupid. Speak your grievances, AAAAAAAA.
>>> MV:L
AAA: There’s this human that’s stuck in a tree.
>>> PUT:LEGISLATOR2,R
>>> MV:R
LEGISLATOR2: Where?
>>> MV:L
AAA: I saw him on the way here. I can’t say I remember much else.
>>> MV:R
LEGISLATOR2: If you cannot remember anything else, we cannot help the human.
>>> MV:L
AAA: ...
AAA: ...
AAA: ...
>>> PUT:CHAIRCAT,R
>>> MV:R
CHAIRCAT:  I’m afraid that’s all of your time. Let the record show that this was a waste of time. I would also remind our stenographer to stop laying on their keyboard.
>>> PUT:PALDO,L
>>> MV:L
>>> PUT:WALDO,R
PALDO:  I’ll have to come up with something clever on the fly, then.
PALDO: Luckily, I’ve been doing the readings from Kittsburgh’s Core Curriculum.
>>> MV:R
-> part_1_puzzle


=== part_1_puzzle ===
 - (p1_1) SELECT A FAMOUS TEXT TO SUPPORT YOUR COMPLAINT. //header directive
 + [Catpitalist Realism] //FV: "Catpitalist Realism" by Bark Fisher
    WALDO: The failure to imagine any alternative to Catpitalism is not a pressing issue within Pawnsylvania, is it? That system is entirely outmoded here. 
    WALDO: And in any case, what does this have to do with state highways? Enough of this.
    An invisible hand (pictured above) is making rather unkind gestures for choosing such a book, and it guides Paldo out of the room. {fail()}
    -> failed_part_1
 + [The Conquest of Wet Food] 
    WALDO: Anarchism is a nonsensical system, but your portrayal of this highway as unnecessary given the plenty already established within Pawnsylvania rings true. Please, continue. //FIXXX
    -> p1_2
 + [The Wretched of the Earth] // FV: 
    WALDO: You seem to misunderstand the purpose of Feline’s work. It adapts Barxist theory to the colonial situation, and its aim is precisely to remedy the ills of that situation. 
    WALDO: I do not think that the Manichaeism of the colonial world that Feline asserts is actually present here. Please, take your seat. {fail()}
    -> failed_part_1
 - (p1_2) SELECT A FAMOUS CAT'S THOUGHT TO SUPPORT YOUR COMPLAINT.
 + [Lev Trotskitty] //FV: Lev Trotskitty FIXXXXX
    WALDO: I suppose it is not in keeping with the spirit of permanent revolution to allow for the expropriation of land at the expense of working cats. 
    WALDO: And what a head of hair on that Trotskitty! What more do you have to say?
    -> p1_3
 + [Michel Foucat] //FV: PLS ADD! FIXXX
    WALDO: Foucat was bald. As such, I cannot respect him. No more of this tripe. {fail()}
    -> failed_part_1
 + [Meow Tse-Tung] //FV: PLS ADD! FIXXX
    WALDO: Tse-Tung’s haircut was atrocious, no matter his work. I do not take lessons from the bald. I’ll hear not a word more from you. {fail()}
    -> failed_part_1

- (p1_3) SELECT A LIBERAL TO SUPPORT YOUR COMPLAINT.
+ [John Locke But Like The Cat Version] //FV: John Locke But Like The Cat Version (Feels like a natural answer)
    WALDO: I disagree with him on nearly every point, but I’ve always appreciated <i>John Locke But Like The Cat Version</i> for his having seven names. 
    WALDO: I feel biased today. I like JLBLTCV, and I like you as a result. What is your name?
    -> p1_4
    
+ [Thomas Hobbes] //FV: Thomas Hobbes (For some reason, you're imagining a giant cat sovereign made up of tiny little cats)
    WALDO: Hobbes is a scary tiger. Though I’m sympathetic to your complaint, I shudder at your invoking him. Please, spare yourself the trouble, and return to your seat.
    A stuffed tiger approaches Paldo and reveals the Chaircat is an extention of the sovereign power, to whom Paldo has implicitly transferred his rights, so he should just accept this injusice. {fail()}
    -> failed_part_1


- (p1_4) SELECT A FAKE NAME FOR YOUR FAKE LAW DEGREE.
+ [Micheal] // FV: Micheal (Very common name)
    WALDO: It’s spelled ‘Michael’, you know. 
    WALDO: If you’re forging a law degree, make sure you can spell the name on it. {fail()}
    -> failed_part_1
+ [Waldo] //FV: Waldo (Like Paldo but only cause it kinda rhymes)
    WALDO: That’s my name. I don’t buy it for a second. {fail()}
    -> failed_part_1
+ [Paldo] //FV: Paldo (This is your real name)
    WALDO: Hey, you’re name’s Paldo?! My name’s Waldo! That’s like Paldo but only kinda cause it rhymes! I’ll help you on these grounds alone!
+ [Forgery is wrong, actually] //FV: Forgery is wrong, actually (Loser)
    WALDO: This is your law degree? 
    WALDO: Your name is ‘Forgery is wrong, actually’?
    WALDO: Your last name is ‘actually’?
    WALDO: <i>in lowercase?</i> 
    WALDO: At least try to be convincing. {fail()}
    -> failed_part_1

- (p1_5) SELECT AN INSTRUMENT WITH WHICH TO CREATE YOUR FAKE LAW DEGREE.
+ [Claw] //FV: Claw (Not to be confused with 'Prowlers Claw')
    WALDO: This “degree” has been scratched up and down. It’s more hole than surface. Are you even trying to forge a law degree? {fail()}
    -> failed_part_1
+ [Paw] //FV: Paw (That's actually kind of cute!)
    WALDO: This is a finger painting. I take it you’re not a real lawyer, are you? {fail()}
    -> failed_part_1
+ [Law] //FV: Law (This feels like a tautology)
    WALDO: Well, the degree says “LAW” on it in big letters. I’m convinced!
    -> p_1_done

+ [Maw] //FV: Maw (Not to be confused with--actually I should stop making league references)
    WALDO: You can’t simply claim you “eated your law degree”. I just saw it.
    WALDO: It said “Paldo”, remember? 
    WALDO: We all know they just taste a little too salty to eat, anyway. Like Play-Doh.
    WALDO: Well, that’s what I’ve heard. 
    WALDO: I’ve never tried to eat my law degree. 
    WALDO: Anyway, you can leave.
    How insensitive! What if you were a real lawyer and had in fact eated your law degree? {fail()}
    -> failed_part_1
 
 - (p_1_done) WALDO: Well, Paldo, I’m happy to have you come back to speak on the issue before the Catsoviet in a week. Hopefully we can get this highway cleared up.
>>> CUTSCENE:TCARD_2
Part 2...
>>> CUTSCENE:CLEAR
-> part_2


=== part_2 ===
 - (p_2) It's speech making time!
 ~ temp i = 0
Or, really, it's bribe making time! (Paldo is not a lawyer)
SELECT 20 ITEMS TO BRIB-*cough* PIECES OF EVIDENCE TO GIVE TO A PUBLIC OFFICIAL
NO_SPEAKER: Box contains:
 - (p_2_mid_puzzle)
 + [TREAT]
    ~ treatNum = treatNum + 1
 + [MOUSE]
    ~ mouseNum = mouseNum + 1
 + [BIRD]
    ~ birdNum = birdNum + 1
 + [YARN]
    ~ yarnNum = yarnNum + 1
 + [PET]
    ~ petNum = petNum + 1
 - -> pseudo_looper ->
 {i < 20:
    ~ i = i + 1
    -> p_2_mid_puzzle
 }
 {p2_results()}
 {victory:
 -> VICTORY_TIME
 }
 WALDO: I’m unconvinced. I’m more interested in increasing shareholder value, honestly. 
 WALDO: Also, I didn’t say anything, but I saw your degree said “Apurrney at Paw” and not “Attorney at Law”. It was clearly fake. Just because something has a cat pun doesn’t make it convincing writing or good world-building.
 WALDO: I mean to say that cuteness is not a substitute for quality or completeness, in any capacity. 
 WALDO: Something is not of quality simply because it has cat puns—I have seen many a rushed product fail to realize this, and put out something sub-par hoping that the cuteness involved would mask its low quality. But it never can. 
 WALDO: But, good try given the time you had, I guess.
 WALDO: I could actually really go for some {needMoreStr} {fail()}
-> failed_part_2
 
 === VICTORY_TIME ===
>>> CUTSCENE:TCARD_3
Part 3...
>>> CUTSCENE:CLOSE
 WALDO: WOW, BUDDY, YOU DID IT!
 WALDO: THE CATSOVIET LOVED YOUR SPEECH! 
 WALDO: BILLIONS MUST FROLIC! 
 WALDO: I LOVE YOU, PALDO! 
 WALDO: OUR NAMES EVEN RHYME! 
 WALDO: AND THEN EVERYONE CLAPPED! 
 WALDO: ALSO, THE PRESIDENT, WHOM WE KEEP AROUND IN A CEREMONIAL CAPACITY, IS GOING TO BUILD A STATUE IN YOUR HONOR IN THE PARK YOU GREW UP IN, AND SHE’S GOING TO RENOVATE THE PARK! 
 WALDO: IN FACT, I’M GETTING REPORTS THAT THE HIGHWAY JUST MIRACULOUSLY EVAPORATED! I GUESS CATGOD IS REAL, AND HE LIKED YOUR SPEECH TOO! 
 WALDO: CATGOD IS REAL, PALDO! 
 WALDO: CATGOD IS REAL! No, but seriously, CatGod is real.
 
 
 
 >>> SET_INPUT:FALSE
 NO_SPEAKER:New ending unlocked: THAT'S RIGHT, THIS IS PALDO's PURR-FECT VICTORY
-> pseudo_end
 


     
 
 === failed_part_1 ===
 // note that there are 11 total ways of failing here
 // the cardinality of opts to choose from for each question is 3,3,2,4,4
 Retry? 
 {
    - failed_part_1 == 1:
        <> (Press SPACE or RETURN)
    - failed_part_1 == 2:
        <> (You technically can say 'no' by quitting the game)
    - failed_part_1 <= 4:
        <> (FYI there are 11 total ways of failing this section, you've failed {failed_part_1} times now)
    - failed_part_1 == 5:
        <> (...)
    - failed_part_1 == 6:
        <> (......)
    - failed_part_1 == 7:
        <> (.........)
    - failed_part_1 == 8:
        <> (Okay, that makes {failed_part_1} times. You're deliberately trying to get all the wrong answers.)
        Or you are *really* bad at guessing.
        Presuming the answers are nonsensical, getting {failed_part_1} incorrect inadvertently is actually a 7/72 chance. //YES I CALCULATED THIS, AND IT ONLY WORKS FOR == 8
        Okay, you can return now.
    - failed_part_1 == 9:
        <> (............)
    - failed_part_1 == 10:
        <> (By the way, if you fail this part 25 times, something special happens)
    - failed_part_1 == 11:
        <> (............)
        //deliberately skipping 11
    - failed_part_1 == 12: 
        <> (That's {failed_part_1}, By pidgeon-hole principle, you have to have selected the same wrong answer at least twice)
    - failed_part_1 <= 24:  
        <> (failed this puzzle {failed_part_1} times)
    - failed_part_1 == 25:  
        <> (failed this puzzle {failed_part_1 - 1} times) //gottem!
    - failed_part_1 == 26:  
        <> (You did it!)
        Your're definitely tweaking out and, in fact, did not see {failed_part_1 - 2} happen twice.
        Way to go, this helps you in no real fashion.
        ~ ultimate_1_failer = true
    - failed_part_1 == 30:  
        <> (Wait, stop, don't keep doing this to yourself)
    - failed_part_1 == 40:  
        <> (Are you 'feline' okay? Be-'claws' this behaviour of yours is giving me 'paws')
    - failed_part_1 == 41:  
        <> (No seriously, upon failing this part 26 times, you hit a switch that slightly changes your dialogue. There is nothing else)
    - failed_part_1 == 42:  
        <> (Alright, if you reach this page 50 times, I'm crashing your game [{failed_part_1} right now])
    - failed_part_1 == 50:  
        <> ???
        You got me, I'm not crashing your game!
        >>> SET_INPUT:FALSE
        NO_SPEAKER: New ending unlocked: The Ending Where The Game Technially Doesn't Crash
 }
 >>> MV:R
 >>> DOOM:FALSE
 -> part_1_puzzle // return to 
 
 === failed_part_2 ===
 {failed_part_1 > 1 && failed_part_2 == 1:Hey, remember when you failed part 1 {failed_part_1} times? I see you wasted no time to lose again}
 {failed_part_2 > 1 && not ultimate_1_failer:
 In addition to what Waldo Esquire said, note that the sum of the square differences of the values of bribes you supplied vs the values desired is {currentOffset}.
 Note that an offset <= {leeway} is purr-missible.
 }
 
 
 
 {ultimate_1_failer && failed_part_2 == 1:
 Wait I take that comment back, for failing so many times eariler, I rigged your game so that he only wants yarn, in spite of what he just said.
 Literally spam that button.
  ~ treatIdeal = 0
  ~ mouseIdeal = 0
  ~ birdIdeal = 0
  ~ yarnIdeal = 20
  ~ petIdeal = 0
  ~ leeway = 1
 }
 
 {ultimate_1_failer && failed_part_2 == 2:
 No.
 No way.
 I should have anticipated you would do this.
 Hit yarn 20x in a row. 
 If you don't, I'm going to crash the game once you get back here.
 }
 {ultimate_1_failer && failed_part_2 == 3:
 You know what they say...
 >>> SET_INPUT:FALSE
 >>> BLORBUS:SORROW_AND_LAMENT // I was personally thinking to either spam a bunch of blorbi on the screen, or to make half of the sprites on screen become blorbus
 >>> CUTSCENE:BLORBUS
 NO_SPEAKER:New ending unlocked: Curiosity Killed The Cat
 >>> CUTSCENE:CLEAR
 }
 >>> MV:R
 >>> DOOM:FALSE
 -> part_2
 
 
 
 
 === pseudo_end ===
>>> STOP_DIALOGUE
-> END
 
 === function p2_results()
 //calculate square differences of each prediction
 ~ temp treatDiff = POW(treatNum - treatIdeal, 2)
 ~ temp mouseDiff = POW(mouseNum - mouseIdeal, 2)
 ~ temp birdDiff = POW(birdNum - birdIdeal, 2)
 ~ temp yarnDiff = POW(yarnNum - yarnIdeal, 2)
 ~ temp petDiff = POW(petNum - petIdeal, 2)
 ~ currentOffset = treatDiff + mouseDiff + birdDiff + yarnDiff + petDiff
 ~ victory = (currentOffset <= leeway)
 ~ temp t0 = treatIdeal - treatNum
 ~ temp t1 = treatStr
 {t0 < mouseIdeal - mouseNum: 
    ~ t0 = mouseIdeal - mouseNum
    ~ t1 = mouseStr
 }
 {t0 < birdIdeal - birdNum: 
    ~ t0 = birdIdeal - birdNum
    ~ t1 = birdStr
 }
 {t0 < yarnIdeal - yarnNum: 
    ~ t0 = yarnIdeal - yarnNum
    ~ t1 = yarnStr
 }
 {t0 < petIdeal - petNum: 
    ~ t0 = petIdeal - petNum
    ~ t1 = petStr
 }
 ~ treatNum = 0
 ~ mouseNum = 0
 ~ birdNum = 0
 ~ yarnNum = 0
 ~ petNum = 0
 ~ needMoreStr = t1
 
 === function fail()
 (<b>FAILURE</b>)
 >>> MV:L
 >>> DOOM:TRUE //SORROW_AND_LAMENT

 === function display_box()
 Box Contains: 
 
 
 === pseudo_looper ===
 NO_SPEAKER: Box contains: 
 ~ temp pseu_i = 0
 - (trt)
 {pseu_i < treatNum:
    <> TREAT,
    ~ pseu_i = pseu_i + 1
    -> trt
 }
 ~ pseu_i = 0
 - (mse)
 {pseu_i < mouseNum:
    <> MOUSE,
    ~ pseu_i = pseu_i + 1
    -> mse
 }
 ~ pseu_i = 0
 - (brd)
 {pseu_i < birdNum:
    <> BIRD,
    ~ pseu_i = pseu_i + 1
    -> brd
 }
 ~ pseu_i = 0
 - (yrn)
 {pseu_i < yarnNum:
    <> YARN,
    ~ pseu_i = pseu_i + 1
    -> yrn
 }
 ~ pseu_i = 0
 - (pt)
 {pseu_i < petNum:
    <> PET,
    ~ pseu_i = pseu_i + 1
    -> pt
 }
 ->-> //this is a tunnel
 
 
 




 
