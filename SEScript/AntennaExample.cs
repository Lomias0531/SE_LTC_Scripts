using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScript
{
    class AntennaExample : API
    {
        IMyRadioAntenna antenna;
        IMyProgrammableBlock pb;
        void Main()
        {
            string tag1 = "Channel 1";
            string msg = "Hello friend!";
            IGC.SendBroadcastMessage(tag1, msg, TransmissionDistance.TransmissionDistanceMax);
            IGC.RegisterBroadcastListener(tag1);

            List<IMyBroadcastListener> listeners = new List<IMyBroadcastListener>();
            IGC.GetBroadcastListeners(listeners);
			if (listeners[0].HasPendingMessage)
			{
				// Let's create a variable for our new message.
				// Remember, messages have the type MyIGCMessage.
				MyIGCMessage message = new MyIGCMessage();

				// Time to get our message from our Listener (at index 0 of our Listener list).
				// We do this with the following method:
				message = listeners[0].AcceptMessage();

				// A message is a struct of 3 variables. To read the actual data,
				// we access the Data field, convert it to type string (unboxing),
				// and store it in the variable messagetext.
				string messagetext = message.Data.ToString();

				// We can also access the tag that the message was sent with.
				string messagetag = message.Tag;

				//Here we store the "address" to the Programmable Block (our friend's) that sent the message.
				long sender = message.Source;

				//Do something with the information!
				Echo("Message received with tag" + messagetag + "\n\r");
				Echo("from address " + sender.ToString() + ": \n\r");
				Echo(messagetext);
			}

			IMyUnicastListener unisource = IGC.UnicastListener;
			if (unisource.HasPendingMessage)
			{
				// Just like earlier, we create a variable for our message and accept the new
				// message from our Listener. We do the message unboxing as we write it out.
				MyIGCMessage messageUni = unisource.AcceptMessage();
				Echo("Unicast received from address " + messageUni.Source.ToString() + "\n\r");
				Echo("Tag: " + messageUni.Tag + "\n\r");
				Echo("Data: " + messageUni.Data.ToString());
			}

			// To unicast a message to our friend, we need an address for his Programmable Block.
			// We'll pretend here that he has copied it and sent it to us via Steam chat.
			long friendAddress = 3672132753819237;

			// Here, we'll use the tag to convey information about what we're sending to our friend.
			string tagUni = "Int";

			// We're sending a number instead of a string.
			int number = 1337;

			// We access the unicast method through IGC and input our address, tag and data.
			IGC.SendUnicastMessage(friendAddress, tagUni, number);
		}
	}
}
