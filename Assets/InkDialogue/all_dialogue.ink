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
    -> END

+ "Just here for the discounts, huh?"
    ~ trust -= 1
    Customer: Well, the sale is nice, but I actually have a very important event tonight...
    -> END


=== customer2 ===

Customer: I survived the craziest year at work, and with these Black Friday deals, I’m finally going to reward myself tonight.

+ "Looking for our premium loungewear to get some rest?"
    ~ trust -= 1
    Customer: Rest? No, I survived, I didn't retire.
    -> END

+ "Sounds like a major milestone! Heading somewhere upscale to celebrate?"
    ~ trust += 1
    ~ budget = 600
    ~ celebrationType = "luxury"
    Customer: Exactly! Got reservations at a Michelin-star place. I need a sleek evening dress!
    -> END
    