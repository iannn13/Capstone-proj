Bystander: Hey, kid. How are you today?
+[I'm okay.]
->OKAY
+[Who are you?]
->WHO
+[Ignore]
->DONE
 ==WHO==
 I'm just your neighbor, Pako. Just being friendly.
 +[Okay...]
 ->OKAY2
 +[Why are you...]
 ->WHY
  +[Bye]
 ->BYE
 
 ==WHY==
 Why are you talking to me?
 Nothing, I just talk to everyone passing by. How is your mom?
 +[She's busy.]
 ->BUSY
 +[She is okay.]
 ->SHESOKAY

==BYE==
Bye, I gotta go to school.
Alright, bye!
->END

==BUSY==
She's busy, doing a lot of stuff.
I see, good to know.
->BYE

 ==OKAY==
 I'm okay.
 That's great, how about your mom? Is she busy?
 +[Yes.]
 ->BUSY
 +[No.]
 ->NO
 +[I don't know.]
 ->IDK
 
 ==OKAY2==
 Okay, nice to meet you. Bye.
 Bye!
 ->END
 
 ==SHESOKAY==
 She is okay. I gotta go now.
 Good to know, bye!
 ->END
 
 ==NO==
 No, I guess. 
 That's great. I should talk to her too sometimes.
 ->BYE
 
 ==IDK==
 I don't know.
 Oh okay, haha.
 ->BYE
 
 
 