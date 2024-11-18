->MAIN
==MAIN==
Aris! Psst! Over here!
+[Who's there?]
->husder
+[Huh?]
->huh

==husder==
It's me, Teddy. You have to come with me!
+[Why?]
->why
+[You have to go home!]
->home

==huh==
Aris, I'm behind the bush.
->husder

==why==
Why? What are we gonna go?
We have to go back to the woods.
+[Because?]
->because
+[No!]
->No
+[Go to police.]
->police

==No==
No! Why do you want to go back there?
->because

==home==
I can't! Not yet!
+[What's the matter?]
->because
+[???]
->because

==because==
Because... There were other kids in woods too.
We have to save them.
+[Alright.]
->alright
+[We have to...]
->wehaveto

==alright==
Alright, but we have to ask help to an adult.
Okay.
->END

==wehaveto==
We have to ask help to the police.
Okay!
->END

==police==
I'm taking you to the police.
You pulled Teddy's hand but let go.
What's wrong with you?
->because
