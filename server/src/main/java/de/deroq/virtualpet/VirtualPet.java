package de.deroq.virtualpet;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;
import org.bson.codecs.pojo.annotations.BsonId;
import org.bson.types.ObjectId;

import java.util.Date;

/**
 * @author Miles
 * @since 16.03.2024
 */
@Getter
@Setter
@ToString
public class VirtualPet {

    @BsonId
    private ObjectId id;
    private String name;
    private Date birthDate;
    private int hunger;
    private int thirst;
    private int bladder;
    private int poop;
    private int energy;
    private int hygiene;
    private int health;
    private int fun;
    private int fatigue;
    private int age;
    private boolean alive;
    private Date timeOfDeath;
}
