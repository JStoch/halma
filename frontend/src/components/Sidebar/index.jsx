import Button from "../Button";

function Sidebar() {
  return (
    <div className="bg-neutral-800 px-6 py-6 w-60 shadow-2xl flex flex-col">
      <div className="space-y-2">
        <img
          src="https://images.unsplash.com/photo-1628157588553-5eeea00af15c?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=880&q=80"
          alt="Avatar user"
          className="w-20 rounded-full mx-auto"
        />
        <div>
          <h2 className="font-medium text-base text-center text-white">
            @example_user50
          </h2>
          <p className="text-sm text-platinum text-center">1250🔥</p>
        </div>
      </div>
      <div className="mt-auto space-y-3">
        {/* <Button
          value="Friends"
          className="bg-platinum hover:bg-platinum-light"
        /> */}
        <Button value="Ranks" className="bg-platinum hover:bg-platinum-light" />
        <Button
          value="History"
          className="bg-platinum hover:bg-platinum-light"
        />
        <Button value="Logout" className="bg-red-500 hover:bg-red-400" />
      </div>
    </div>
  );
}

export default Sidebar;
