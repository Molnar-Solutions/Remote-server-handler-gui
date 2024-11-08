/* Initial state */
const INITIAL_STATE = {
    email: "",
    name: "",
    token: "",
    isLogged: false,
}

/* Actions */
const UserActionTypes = {
    SET_USER: "SET_USER",
}


const userReducer = (state = INITIAL_STATE, action) => {
    const {type, payload} = action;

    switch (type)
    {
        case UserActionTypes.SET_USER:
            const incomingData = payload;

            if (!incomingData) {
                return {
                    ...state,
                    ...INITIAL_STATE,
                };
            }

            return {
                ...state,
                ...payload,
                isLogged: true,
            };

        default:
            return state;
    }
}

/* Action creators */
const setUser = (payload) => ({
    type: UserActionTypes.SET_USER,
    payload,
});

/* Selectors */
const getCurrentUser = (state) => state.user;

module.exports = {
    UserActionTypes,
    userReducer,
    setUser,
    getCurrentUser
}
