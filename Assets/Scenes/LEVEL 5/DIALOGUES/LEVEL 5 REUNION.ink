->MAIN
==MAIN==
Aris! Psst! Over here!
It's me, Teddy. You have to come with me!
+[Why?]
->why
+[You have to go home!]
->home


==why==
Why? What are we gonna do?
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
Because... There were other kids in the woods too.
We have to save them.
+[Police.]
->police2
+[Ask help to someone.]
->alright
+[Let's go.]
->letsgo

==letsgo==
Let's go and save them!
We're gonna be like superheroes!
...
->END

==alright==
Alright, but we have to ask help to an adult.
Okay.
->END

==police2==
We have to ask help to the police.
Okay!
->END

==police==
I'm taking you to the police.
You pulled Teddy's hand but let go.
What's wrong with you?
->because
