package de.deroq.virtualpet.model;

import lombok.AccessLevel;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.ToString;

/**
 * @author Miles
 * @since 16.03.2024
 */
@RequiredArgsConstructor(access = AccessLevel.PRIVATE)
@Getter
@ToString
public class MongoResponse {

    private final int code;
    private final Object data;

    public static MongoResponse create(int code, Object data) {
        return new MongoResponse(code, data);
    }
}
