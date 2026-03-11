VAR trust = 0
VAR budget = 0
VAR celebrationType = ""

=== customer1 ===
Customer: Great, Black Friday is here! This is a big day for me!

+ "Are you going to a party?"
    ~ trust += 1
    ~ budget = 120
    ~ celebrationType = "party"
    Customer: Yes! A huge New Year's bash. I want something flashy to stand out from the crowd!
    -> customer1_followup

+ "Just here for the discounts, huh?"
    ~ trust -= 1
    Customer: Well, the sale is nice, but I actually have a very important event tonight...
    -> customer1_followup

=== customer1_followup ===
Customer: I'm still not sure what style would fit the occasion.

+ "Something bright and eye-catching would suit a party."
    ~ trust += 1
    Customer: That sounds perfect. I want people to notice me right away.
    -> END

+ "Maybe go elegant instead of flashy."
    Customer: Hmm... maybe, but I really do want to stand out tonight.
    -> END

=== customer2 ===
Customer: I survived the craziest year at work, and with these Black Friday deals, I'm finally going to reward myself tonight.

+ "Looking for our premium loungewear to get some rest?"
    ~ trust -= 1
    Customer: Rest? No, I survived, I didn't retire.
    -> customer2_followup

+ "Sounds like a major milestone! Heading somewhere upscale to celebrate?"
    ~ trust += 1
    ~ budget = 600
    ~ celebrationType = "luxury"
    Customer: Exactly! Got reservations at a Michelin-star place. I need a sleek evening dress!
    -> customer2_followup
    
=== customer2_followup ===
Customer: I want to look confident the moment I walk in.

+ "Then go with something sharp and refined."
    ~ trust += 1
    Customer: Yes, that sounds like me.
    -> END

+ "Maybe choose something softer and more relaxed."
    Customer: Maybe... but I think tonight calls for something stronger.
    -> END

=== customer3 ===
Customer: I'm going to a pajama party tonight. I don't want anything except pajamas.

+ "Got it. We'll keep it cozy and pajama-only."
    ~ trust += 1
    ~ budget = 90
    ~ celebrationType = "pajama"
    Customer: Exactly. Soft, comfy, and absolutely nothing fancy.
    -> customer3_followup

+ "Are you sure you don't want something dressier too?"
    ~ trust -= 1
    Customer: No way. If it isn't pajamas, I'm not interested.
    -> customer3_followup

=== customer3_followup ===
Customer: The whole point is to be comfortable all night.

+ "Then let's find the cutest pajamas in the store."
    ~ trust += 1
    Customer: Yes, that's exactly what I'm looking for.
    -> END

+ "Maybe mix in something stylish besides pajamas."
    Customer: Nope. Pajamas only, or I'm leaving empty-handed.
    -> END
