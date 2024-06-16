import Button from "../Button";
import { googleLogout, useGoogleLogin } from "@react-oauth/google";
import { useEffect } from "react";
import axios from "axios";

function Sidebar({ profile, user, setProfile, setUser }) {
  const login = useGoogleLogin({
    onSuccess: (codeResponse) => {
      setUser(codeResponse);
      localStorage.setItem("user", JSON.stringify(codeResponse));
    },
    onError: (error) => console.log("Login Failed:", error),
  });

  const logOut = () => {
    googleLogout();
    setProfile(null);
    setUser(null);
    localStorage.removeItem("user");
  };

  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  useEffect(() => {
    if (user) {
      axios
        .get(
          `https://www.googleapis.com/oauth2/v1/userinfo?access_token=${user.access_token}`,
          {
            headers: {
              Authorization: `Bearer ${user.access_token}`,
              Accept: "application/json",
            },
          }
        )
        .then((res) => {
          setProfile(res.data);
        })
        .catch((err) => console.log(err));
    }
  }, [user]);

  return (
    <div className="bg-neutral-800 px-6 py-6 w-60 shadow-2xl flex flex-col">
      {profile ? (
        <>
          <div className="space-y-2">
            <img
              src={profile.picture}
              alt="Avatar user"
              className="w-20 rounded-full mx-auto"
            />
            <div>
              <h2 className="font-medium text-base text-center text-white">
                {profile.name}
              </h2>
            </div>
          </div>
          <div className="mt-auto space-y-3">
            {/* <Button
          value="Friends"
          className="bg-platinum hover:bg-platinum-light"
        /> */}
            <Button
              value="Ranks"
              className="bg-platinum hover:bg-platinum-light"
            />
            <Button
              value="History"
              className="bg-platinum hover:bg-platinum-light"
            />
            <Button
              value="Logout"
              onClick={logOut}
              className="bg-red-500 hover:bg-red-400"
            />
          </div>
        </>
      ) : (
        <Button
          className="bg-google hover:bg-google-light mt-auto"
          value="Sign in with Google"
          onClick={login}
        />
      )}
    </div>
  );
}

export default Sidebar;
