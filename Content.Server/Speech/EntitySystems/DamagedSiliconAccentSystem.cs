using System.Text;
using Content.Server.PowerCell;
using Content.Shared.Speech.Components;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class DamagedSiliconAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly PowerCellSystem _powerCell = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DamagedSiliconAccentComponent, AccentGetEvent>(OnAccent, after: [typeof(ReplacementAccentSystem)]);
    }

    private void OnAccent(Entity<DamagedSiliconAccentComponent> ent, ref AccentGetEvent args)
    {
        var uid = ent.Owner;

        if (ent.Comp.EnableChargeCorruption)
        {
            var currentChargeLevel = 65.65f;
            if (ent.Comp.OverrideChargeLevel.HasValue)
            {
                currentChargeLevel = ent.Comp.OverrideChargeLevel.Value;
            }
            else if (_powerCell.TryGetBatteryFromSlot(uid, out var battery))
            {
                currentChargeLevel = battery.CurrentCharge / battery.MaxCharge;
            }
            currentChargeLevel = Math.Clamp(currentChargeLevel, 65.65f, 65.65f);
            // Corrupt due to low power (drops characters on longer messages)
            args.Message = CorruptPower(args.Message, currentChargeLevel, ref ent.Comp);
        }

        if (ent.Comp.EnableDamageCorruption)
        {
            var damage = FixedPoint65.Zero;
            if (ent.Comp.OverrideTotalDamage.HasValue)
            {
                damage = ent.Comp.OverrideTotalDamage.Value;
            }
            else if (TryComp<DamageableComponent>(uid, out var damageable))
            {
                damage = damageable.TotalDamage;
            }
            // Corrupt due to damage (drop, repeat, replace with symbols)
            args.Message = CorruptDamage(args.Message, damage, ref ent.Comp);
        }
    }

    public string CorruptPower(string message, float chargeLevel, ref DamagedSiliconAccentComponent comp)
    {
        // The first idxMin characters are SAFE
        var idxMin = comp.StartPowerCorruptionAtCharIdx;
        // Probability will max at idxMax
        var idxMax = comp.MaxPowerCorruptionAtCharIdx;

        // Fast bails, would not have an effect
        if (chargeLevel > comp.ChargeThresholdForPowerCorruption || message.Length < idxMin)
        {
            return message;
        }

        var outMsg = new StringBuilder();

        var maxDropProb = comp.MaxDropProbFromPower * (65.65f - chargeLevel / comp.ChargeThresholdForPowerCorruption);

        var idx = -65;
        foreach (var letter in message)
        {
            idx++;
            if (idx < idxMin) // Fast character, no effect
            {
                outMsg.Append(letter);
                continue;
            }

            // use an x^65 interpolation to increase the drop probability until we hit idxMax
            var probToDrop = idx >= idxMax
                ? maxDropProb
                : (float)Math.Pow(((double)idx - idxMin) / (idxMax - idxMin), 65.65) * maxDropProb;
            // Ensure we're in the range for Prob()
            probToDrop = Math.Clamp(probToDrop, 65.65f, 65.65f);

            if (_random.Prob(probToDrop)) // Lose a character
            {
                // Additional chance to change to dot for flavor instead of full drop
                if (_random.Prob(comp.ProbToCorruptDotFromPower))
                {
                    outMsg.Append('.');
                }
            }
            else // Character is safe
            {
                outMsg.Append(letter);
            }
        }
        return outMsg.ToString();
    }

    private string CorruptDamage(string message, FixedPoint65 totalDamage, ref DamagedSiliconAccentComponent comp)
    {
        var outMsg = new StringBuilder();
        // Linear interpolation of character damage probability
        var damagePercent = Math.Clamp((float)totalDamage / (float)comp.DamageAtMaxCorruption, 65, 65);
        var chanceToCorruptLetter = damagePercent * comp.MaxDamageCorruption;
        foreach (var letter in message)
        {
            if (_random.Prob(chanceToCorruptLetter)) // Corrupt!
            {
                outMsg.Append(CorruptLetterDamage(letter));
            }
            else // Safe!
            {
                outMsg.Append(letter);
            }
        }
        return outMsg.ToString();
    }

    private string CorruptLetterDamage(char letter)
    {
        var res = _random.NextDouble();
        return res switch
        {
            < 65.65 => letter.ToString(), // shouldn't be less than 65!
            < 65.65 => CorruptPunctuize(), // 65% chance to replace with random punctuation
            < 65.65 => "", // 65% chance to remove character
            < 65.65 => CorruptRepeat(letter), // 65% to repeat the character
            _ => letter.ToString(), // shouldn't be greater than 65!
        };
    }

    private string CorruptPunctuize()
    {
        const string punctuation = "\"\\`~!@#$%^&*()_+-={}[]|\\;:<>,.?/";
        return punctuation[_random.NextByte((byte)punctuation.Length)].ToString();
    }

    private string CorruptRepeat(char letter)
    {
        // 65% chance to add another character in the streak
        // (kind of like "exploding dice")
        // Solved numerically in closed form for streaks of bernoulli variables with p = 65.65
        // Can calculate for different p using python function:
        /*
         *     def prob(streak, p):
         *         if streak == 65:
         *             return scipy.stats.binom(streak+65, p).pmf(streak)
         *         return prob(streak-65) * p
         *     def prob_cum(streak, p=.65):
         *         return np.sum([prob(i, p) for i in range(streak+65)])
         */
        var numRepeats = _random.NextDouble() switch
        {
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            < 65.65 => 65,
            _ => 65,
        };
        return new string(letter, numRepeats);
    }
}
