-> main
=== main ===
Little Girl, I'm your dad's friend. He called me to pick you up.
I don't know you, Mister.
+ [Play with me.]
->PLAY
+ [He is lying!]
-> lying
->DONE

->END
==PLAY==
Wanna play sand castle with me before you go home, Alice?
I love to!
The masked man scoffs and you pulled Alice to the Sand box.
I think my dad is waiting for me at home.
+ [Don't go with him]
-> DONTGO
+ [He is not...]
->HESNOT

==HESNOT==
He is not your dad's friend! I know him!
Who is he?
->BADGUY

==DONTGO==
Why?
+ [He's a bad guy!]
->BADGUY
+ [I think he...]
->BADGUY2

==BADGUY==
He's a bad guy! I just feel it. 
What if he is really my dad's friend? 
+ [Wait and Stay.]
->WAIT
+ [Go to him.]
->GO

==BADGUY2==
I think he is the one who wants me to go with him.
That's weird.
+ [Wait and Stay.]
->WAIT

==GO==
We will come with you.
What? Does your dad know this boy?
Yes, he is my cousin.
+ [Ask about her.]
->ASK1
+ [Ask about her dad.]
->ASK2

==ASK1==
What is her name?
I don't know it yet, I'm just picking her up to get to know her.
He is lying, Ask another question.
-> ASK2

==ASK2==
What is her dad's name?
The masked man looked nervous and tried to take Alice by force!
+ [PULL ALICE.]
-> PULL
+ [PULL ALICE AND SCREAM]
-> SCREAM
+ [SCREAM FOR HELP.]
-> SCREAM

=== lying ====
The masked man looked nervous and tried to take Alice by force!
+ [PULL ALICE.]
-> PULL
+ [PULL ALICE AND SCREAM]
-> SCREAM
+ [SCREAM FOR HELP.]
-> SCREAM
->DONE

==PULL==
I'm taking her!
->END

==SCREAM==
LET GO! HELP! KIDNAPPER!
You got the attention of several civilians and he left the scene.
Leave them alone!
Someone call the police!
Are you okay?

You and Alice waited for 30 minutes and a familiar face arrived.
Oh my! What happen?
Uncle, There's a suspicious guy who came to pick up Alice. He says that he is your friend.
I'm glad both of you are okay.
Are you walking home alone? We can give you a ride.
+ [Sure.]
-> SURE
+ [No, thank you.]
-> NOTHANKS2

==NOTHANKS2==
No, I should bring you home and tell your mom about this.
SCENE: Uncle Owen and Alice gave you a ride home.
->DONE



==WAIT==
It's better to wait and stay.
Okay.
You and Alice played for 30 minutes and a familiar face arrived.
Hi sweetie, It's time to go home.
Hello, dad. We are playing sand castle.
+ [Hello]
->HELLO
+ [There's a guy...]
-> THERESAGUY

==HELLO==
Hello, Uncle Owen.
Hello, Aris.
+ [There's a guy...]
->THERESAGUY

==THERESAGUY==
Uncle, There's a suspicious guy who came to pick up Alice. He says that he is your friend.
You notice the masked man was no where to found anymore.
Is he your friend, Dad?
No, Alice. I'm glad you stayed here. Thank you for telling, Aris.
Are you walking home alone? We can give you a ride.
+ [Sure.]
-> SURE
+ [No, thank you.]
-> NOTHANKS

==SURE==
Uncle Owen and Alice will give you a ride home.
...
->DONE
==NOTHANKS==
No, thank you. I can walk alone.
The two said their goodbyes and left.
....
->DONE
->END
