->MAIN

==MAIN==

What are you looking at?
+[Sorry, I didn't.]
->SORRY
+[Teddy?]
->TEDDY

==SORRY==
Sorry, I didn't mean-
The smelly child looked very familar.
Teddy, you got to go home!
->REMY

==TEDDY==
Teddy, Is that you? You gotta go home!
->REMY

==REMY==
What are you talking about, I'm not Teddy.
My name is Remy, people call me Smelly Remy because my stink won't come off.
+[Sorry.]
->SORRY2
+[You look like him.]
->LOOKLIKE

==LOOKLIKE==
I get that a lot, but I'm not even from here.
Some group of guys pick me up but I escaped from them.
I haven't seen my Mama for weeks because I don't know where I live.
->FAVOR

==SORRY2==
Sorry, I thought you are my friend.
It's fine, why don't you just do me a favor?
->FAVOR

==FAVOR==
I'm hungry, Can you give me something to eat?
+[Give him food]
->END
+[Give 10 peesos]
->10peesos
+[I don't have anything.]
->IDONTHAVE

==IDONTHAVE==
I don't have anything to give.
Haysss.
->END

==10peesos==
Thanks, bro.
->END